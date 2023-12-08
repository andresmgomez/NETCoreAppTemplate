using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Areas.Admin.Models;
using TemplateRESTful.Web.Controllers;
using static TemplateRESTful.Web.Areas.Admin.Models.AccountViewModel;

namespace TemplateRESTful.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : RootController<AccountsController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ActionResult> Index()
        {
            var accountUsers = await _userManager.Users.ToListAsync();
            var acoountViemModel = new List<AccountViewModel>();

            foreach (var account in accountUsers)
            {
                var accountModel = new AccountViewModel();

                accountModel.FirstName = account.FirstName;
                accountModel.LastName = account.LastName;
                accountModel.Email = account.Email;
                accountModel.EmailConfirmed = account.EmailConfirmed;
                accountModel.IsActive = account.IsActive;
                accountModel.Roles = await GetAccountRoles(account);
                acoountViemModel.Add(accountModel);
            }

            return View(acoountViemModel);
        }

        private async Task<List<string>> GetAccountRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

    }
}
