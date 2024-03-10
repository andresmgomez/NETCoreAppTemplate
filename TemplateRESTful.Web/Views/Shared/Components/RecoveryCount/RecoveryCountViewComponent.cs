using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs.User;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Web.Views.Shared.Components.RecoveryCount
{
    public class RecoveryCountViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RecoveryCountViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public RecoveryUserDto RecoveryUser { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userAccount = await _userManager.GetUserAsync(HttpContext.User);

            var trustedUser =  await _userManager.GetTwoFactorEnabledAsync(userAccount);

            if (trustedUser.Equals(true))
            {
                var recoveryCodesUsed = await _userManager.CountRecoveryCodesAsync(userAccount);

                RecoveryUser = new RecoveryUserDto
                {
                    RecoveryCodesLeft = recoveryCodesUsed
                };

                return View("Default", RecoveryUser);
            }

            var initRecoverUser = new RecoveryUserDto
            {
                RecoveryCodesLeft = 8
            };

            return View("Default", initRecoverUser);
        }
    }
}
