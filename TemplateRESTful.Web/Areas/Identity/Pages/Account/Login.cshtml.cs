using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Web.Implementation;
using TemplateRESTful.Infrastructure.Server.Requests.IRepository;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Service.Common.Account;
using MediatR;
using TemplateRESTful.Persistence.Data.Actions;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : ImplementService<PageModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthorizeService _authorize;
        private readonly IEmailRequest _sendEmailRequest;
        private readonly IMediator _mediator;
        
        public LoginModel(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthorizeService authorize,
            IEmailRequest sendEmailRequest, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorize = authorize;
            _sendEmailRequest = sendEmailRequest;
            _mediator = mediator;
        }

        [BindProperty]
        public LoginUserDto Input { get; set; }
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
                    result = await _authorize.LoginUserAsync(Input, allowLockout: false);
            
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
                    result = await _authorize.LoginUserAsync(Input, allowLockout: true);
                    
                    if (result.Succeeded)
                    {
                        
                        user.IsActive = true;
                        await _mediator.Send(new TrackActivityActions() { userId = user.Id, Action = "Login Success" }); 
                        // _logger.LogInformation($"{emailAccount} has logged in successfully");
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
                    await _mediator.Send(new TrackActivityActions() { userId = user.Id, Action = "Login Failed" });
                    
                    _notificationService.ErrorMessage("Invalid Login attempt! Check your email or password and try again");
                }
                else
                {
                    _notificationService.ErrorMessage("Unable to find valid user account! Check credentials and try again");
                }
            }

            return Page();
        }
    }
}
