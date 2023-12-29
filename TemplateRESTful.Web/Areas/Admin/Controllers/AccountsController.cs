using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : RootController<AccountsController>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var regularUsers = await _userManager.Users.ToListAsync();
            var accountUsers = new List<AccountUser>();

            foreach (var user in regularUsers)
            {
                var accountUser = new AccountUser();

                accountUser.FirstName = user.FirstName;
                accountUser.LastName = user.LastName;
                accountUser.Email = user.Email;
                accountUser.EmailConfirmed = user.EmailConfirmed;
                accountUser.IsActive = user.IsActive;
                accountUser.Roles = await GetAccountRoles(user);
                accountUsers.Add(accountUser);
            }

            return View(accountUsers);
        }

        private async Task<List<string>> GetAccountRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

    }
}
