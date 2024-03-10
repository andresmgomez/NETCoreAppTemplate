using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    public class DisplayInfoModel : PageModel
    {
        private UserManager<ApplicationUser> _userManager;

        public DisplayInfoModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> OnlineUsers { get; set; }

        public async Task<PartialViewResult> OnGetViewAllPartial()
        {
            OnlineUsers = await _userManager.Users.ToListAsync();
            var accountUsers = new List<AccountUser>();

            foreach(var identityUser in OnlineUsers)
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
                ViewName = "_ViewAll",
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
