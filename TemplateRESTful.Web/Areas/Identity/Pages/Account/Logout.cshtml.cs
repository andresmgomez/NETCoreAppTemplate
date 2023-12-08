using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Implementation;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : ImplementService<LogoutModel>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _authLog;

        public LogoutModel(
            SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> authLog)
        {
            _signInManager = signInManager;
            _authLog = authLog;
        }

        public async Task<IActionResult> OnGet()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            await _signInManager.SignOutAsync();
            _authLog.LogInformation("User account has logout successfully");
            _notificationService.InfoMessage("Your account has logout successfully");

            return RedirectToPage("/Account/Login");
        }
    }
}
