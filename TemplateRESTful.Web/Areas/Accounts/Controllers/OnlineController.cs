using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class OnlineController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public OnlineController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> UserAccounts { get; set; }

        [HttpGet]
        public async Task<PartialViewResult> OnlineAccounts()
        {
            UserAccounts = await _userManager.Users.ToListAsync();

            var accountUsers = new List<AccountUser>();

            foreach (var identityUser in UserAccounts)
            {
                var accountUser = new AccountUser();
                accountUser.FirstName = identityUser.FirstName;
                accountUser.LastName = identityUser.LastName;
                accountUser.Email = identityUser.Email;
                accountUser.EmailConfirmed = identityUser.EmailConfirmed;
                accountUser.IsActive = identityUser.IsActive;
                accountUser.Roles = await GetAccountRoles(identityUser);

                accountUsers.Add(accountUser);
            }

            return new PartialViewResult
            {
                ViewName = "_ListOnlineAccounts",
                ViewData = new ViewDataDictionary<IList<AccountUser>>(
                    ViewData, accountUsers),
            };
        }

        private async Task<List<string>> GetAccountRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
