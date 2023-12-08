using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using TemplateRESTful.Domain.Models.Account;
using Microsoft.Extensions.Logging;
using TemplateRESTful.Web.Implementation;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Infrastructure.Server.Requests.IRepository;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : ImplementService<ForgotPasswordModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailRequest _sendEmailRequest;
        private readonly ILogger<ForgotPasswordModel> _authLog;

        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager,
            IEmailRequest sendEmailRequest,
            ILogger<ForgotPasswordModel> authLog
            )
        {
            _userManager = userManager;
            _sendEmailRequest = sendEmailRequest;
            _authLog = authLog;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) 
                {
                    _authLog.LogInformation($"User account was not found on the system");
                    _notificationService.ErrorMessage($"The following email account is invalid");

                    return RedirectToPage("Index");
                } else
                {
                    // For more information on how to enable account confirmation and password reset please 
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { 
                            area = "Identity", 
                            code,
                            email = Input.Email,
                            returnUrl 
                        },
                        protocol: Request.Scheme
                    );

                    var sendMessage = new EmailMessage(new string[]
                    {
                        Input.Email
                    },
                    "Request To Change your Credentials from RESTful API services",
                    $@"<h4>Hello, {Input.Email}, you have requested to change your password</h4>
                       <p>If you have requested to reset your password, click on the following 
                       <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>.</p>
                       <p>Otherwise, ignore this email, and report to your administrator if you have not 
                          authorized this change in your account.</p>"
                    );

                    await _sendEmailRequest.SendEmailAsync(sendMessage);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
            }

            return Page();
        }
    }
}
