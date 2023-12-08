using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Implementation;
using TemplateRESTful.Infrastructure.Server.Requests.IRepository;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Service.Common.Account;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : ImplementService<PageModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthorizeService _manageAccount;
        private readonly IEmailRequest _sendEmailRequest;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthorizeService manageAccount,
            ILogger<LoginModel> logger,
            IEmailRequest sendEmailRequest)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _manageAccount = manageAccount;
            _sendEmailRequest = sendEmailRequest;
            _logger = logger;
        }

        [BindProperty]
        public LoginUser Input { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            Microsoft.AspNetCore.Identity.SignInResult result;

            if (ModelState.IsValid)
            {
                var emailAccount = Input.Email;

                var user = await _userManager.FindByEmailAsync(emailAccount);
                var admin = await _userManager.IsInRoleAsync(user, "Administrator");

                if (user != null && admin)
                {
                    user.TwoFactorEnabled = true;
                    result = await _manageAccount.LoginAccountAsync(Input, allowLockout: false);
            
                    if (result.RequiresTwoFactor) 
                    {
                        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
                        var message = new EmailMessage(new string[]
                            {
                                Input.Email
                            },
                            "Login your Account using Two Factor Authentication",
                            $@"<h4>Hello, Admin</h4>
                            <p>We have recently detected that you try to login to your account as an administrator.</p>
                            <p>Copy and pase the following authorization code to continue the logging process.</p>
                            <h4>{token}</h4>
                            <p>Otherwise, ignore this email report it to your network administrator.</p>"
                        );

                        await _sendEmailRequest.SendEmailAsync(message);
                        return RedirectToPage("./LoginWith2fa", new { user, Input.RememberMe, returnUrl });
                    }
                }

                if (user != null)
                {
                    result = await _manageAccount.LoginAccountAsync(Input, allowLockout: true);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"{emailAccount} has logged in successfully");
                        _notificationService.SuccessMessage($"Hello User, you are logged in as {emailAccount}");
                        return LocalRedirect(returnUrl);

                    }
                    else if (result.IsLockedOut)
                    {
                        return RedirectToPage("./Lockout");
                    }

                    ApplicationUser guestUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
                    int failedAttempts = await _signInManager.UserManager.GetAccessFailedCountAsync(guestUser);
                    int totalAttempts = _signInManager.Options.Lockout.MaxFailedAccessAttempts;
                    string alertMessage = $"Login has failed. Remaining attempts {failedAttempts} of {totalAttempts}";

                    ModelState.AddModelError(string.Empty, alertMessage);
                    _notificationService.ErrorMessage("Invalid Login attempt! Check your email or password and try again");
                }
                else
                {
                    _notificationService.ErrorMessage("Unable to find valid user account! Check credentials and try again");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
