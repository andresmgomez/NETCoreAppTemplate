using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs.User;
using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Web.Views.Shared.Components.RecoveryCodes
{
    public class RecoveryCodesViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RecoveryCodesViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public RecoveryUserDto RecoveryUser { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAccount = await _userManager.GetUserAsync(HttpContext.User);
            var setRecoveryCodes = await _userManager.CountRecoveryCodesAsync(userAccount);

            if (setRecoveryCodes == 0 && await _userManager.GetTwoFactorEnabledAsync(userAccount) == true)
            {
                IEnumerable<string> listRecoveryCodes =
                    await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(userAccount, 8);

                string[] recoveryCodes = listRecoveryCodes.ToArray();

                var recoverAccount = new RecoveryUserDto
                {
                    RecoveryCodes = recoveryCodes
                };

                return View("Default", recoverAccount);
            }

            var initRecoveryUser = new RecoveryUserDto
            {
                RecoveryCodes = null
            };

            return View("Default", initRecoveryUser);
        }
    }
}
