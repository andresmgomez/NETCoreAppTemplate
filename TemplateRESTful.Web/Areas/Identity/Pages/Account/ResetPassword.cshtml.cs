using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Implementation;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : ImplementService<ResetPasswordModel>
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public ResetPasswordModel(
            UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public ForgotUser Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(string email, string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }

            Input = new ForgotUser
            {
                Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                Email = email
            };

            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync(ForgotUser accountUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(accountUser.Email);

                if (user != null)
                {
                    IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"{Input.Email} account has changed password successfully");
                        _notificationService.SuccessMessage($"Your credentials have been changed successfully");

                        return RedirectToPage("./ResetPasswordConfirmation");
                    }

                    _logger.LogInformation($"Email account {Input.Email} reset request was not successfull");
                    _notificationService.ErrorMessage("An error has occurred. Unable to reset your password. " +
                        "Contact your adminsitrator for further assitance");

                }
                else
                {
                    _logger.LogInformation($"Invalid attempt on reset password request on {Input.Email} account");
                    _notificationService.ErrorMessage("An error has occurred on your request. Contact your administrator");
                }
            }

            return Page();
        }
    }
}
