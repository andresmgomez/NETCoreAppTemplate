using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Common.Account;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _authorize;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager,
            IAuthorizeService authorize)
        {
            _userManager = userManager;
            _authorize = authorize;
        }

        [TempData]
        public string StatusMessage { get; set; }
        public string RedirectPageUrl { get; set; }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
                
            if (user == null)
            {
                return NotFound($"Unable to find user account with given {userId}");
            }

            var result = await _authorize.ConfirmUserAsync(user, code);                
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Page();
        }

        public IActionResult OnPost(string returnUrl = null)
        {
            RedirectPageUrl = Url.Page(
                "Account/Login",
                pageHandler: null,
                values: new { area = "Identity", returnUrl = returnUrl },
                protocol: Request.Scheme);

            return Page();
        }

    }
}
