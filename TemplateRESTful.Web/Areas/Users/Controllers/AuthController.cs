using System.Collections.Generic;
using System.Threading.Tasks;

using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Service.Client.Interfaces;
using TemplateRESTful.Service.Server.Interfaces;
using TemplateRESTful.Service.Server.Features;
using TemplateRESTful.Service.Client.Actions.Commands;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class AuthController : RootController<AuthController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthorizeService _authorizeService;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            IAuthorizeService authorizeService, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizeService = authorizeService;
            _emailService = emailService;

        }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDto registerUser)
        {
            var existingAccount = await _userManager.FindByEmailAsync(registerUser.Email);
            
            if (ModelState.IsValid)
            {
                if (existingAccount == null)
                {
                    var registerAccount = await _authorizeService.RegisterUserAsync(registerUser);

                    if (registerAccount.Succeeded)
                    {
                        _notificationService.SuccessMessage($"You have successfully created an account");
                        
                        return RedirectToAction(
                            "RegisterConfirmation",
                            "Confirm", new
                            {
                                emailAccount = registerUser.Email,
                            }
                        );
                    }

                    foreach (var error in registerAccount.Errors)
                    {
                        ModelState.TryAddModelError(string.Empty, error.Description);
                    }
                } else
                {
                    _notificationService.ErrorMessage($"The following account is already in our system");
                }

            }

            return View(registerUser);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
 
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDto loginUser, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var userAccount = await _userManager.FindByEmailAsync(loginUser.Email);

            if (ModelState.IsValid)
            {
                if (userAccount != null)
                {
                    var adminAccount = await _userManager.IsInRoleAsync(userAccount, "Administrator");

                    SignInResult userRequest = await _authorizeService.LoginUserAsync(
                        loginUser, true);

                    if (userRequest.RequiresTwoFactor && !adminAccount)
                    {
                        var validProvider = await _userManager.GetValidTwoFactorProvidersAsync(userAccount);

                        if (validProvider.Contains(_userManager.Options.Tokens.AuthenticatorTokenProvider))
                        {
                            return RedirectToAction("SecureLogin", "Secure", new { email = userAccount.Email });
                        }
                    }
                    else if (userRequest.RequiresTwoFactor && adminAccount)
                    {
                        var accessCode = await _userManager.GenerateTwoFactorTokenAsync(userAccount, "Email");

                        var emailMessage = EmailConfirmation.SetEmailContent(userAccount, accessCode);
                        await _emailService.SendEmailAsync(emailMessage);

                        return RedirectToAction("SecureLogin", "Secure", new { email = userAccount.Email });
                    }
                    else if (userRequest.IsLockedOut)
                    {
                        return RedirectToAction("LockoutUser", "Confirm");
                    }
                    else if (userRequest.Succeeded)
                    {
                        userAccount.IsActive = true;
                        await _userManager.UpdateAsync(userAccount);

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        int failedAttempts = await _signInManager.UserManager.GetAccessFailedCountAsync(userAccount);
                        int totalAttempts = _signInManager.Options.Lockout.MaxFailedAccessAttempts;
                        string alertMessage = $"Login has failed. Remaining attempts {failedAttempts} of {totalAttempts}";

                        ModelState.AddModelError(string.Empty, alertMessage);

                        await _mediator.Send(new TrackLoginCommand()
                        {
                            userId = userAccount.Id,
                            Action = "Login Failed"
                        });   
                    }
                }

                _notificationService.ErrorMessage(
                    "Invalid Login attempt! Check your email or password and try again"
                );
                
                return View(loginUser);

            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            foreach(string cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            
            await _signInManager.SignOutAsync();
            _notificationService.SuccessMessage($"Your account has logged out successfully");

            return RedirectToAction("Login", "Auth");
        }
    }
}
