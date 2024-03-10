using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Persistence.Data.Actions;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Service.Common.Identity;
using TemplateRESTful.Service.Common.Email;
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
            if (ModelState.IsValid)
            {
                var existingAccount = await _userManager.FindByEmailAsync(registerUser.Email);

                if (existingAccount == null)
                {
                    var registerAccount = await _authorizeService.RegisterUserAsync(registerUser);

                    if (registerAccount.Succeeded)
                    {
                        _notificationService.SuccessMessage($"You have successfully created a new Account");
                        
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
            Microsoft.AspNetCore.Identity.SignInResult userRequest;

            if (ModelState.IsValid) 
            {
                var userAccount = await _userManager.FindByEmailAsync(loginUser.Email);
                var adminAccount = await _userManager.IsInRoleAsync(userAccount, "Administrator");

                if (userAccount != null && adminAccount)
                {
                    userAccount.TwoFactorEnabled = true;
                    userRequest = await _authorizeService.LoginUserAsync(loginUser, allowLockout: false);

                    if (userRequest.RequiresTwoFactor)
                    {
                        var userAuthCode = await _userManager.GenerateTwoFactorTokenAsync(userAccount, "Email");

                        var emailMessage = new EmailMessage(new string[]
                            {
                                loginUser.Email
                            },
                            "RestfulAPI - Login Your Account with Two Factor Authentication",
                            $@"<h4>Hello, {loginUser.Email}</h4>
                            <p>We have recently detected that you try to login to your account as an administrator.</p>
                            <p>Copy and pase the following authorization code to continue the login process.</p>
                            <h4>{userAuthCode}</h4>
                            <p>Otherwise, ignore this email report it to your network administrator.</p>"
                        );

                        await _emailService.SendEmailAsync(emailMessage);

                        return RedirectToAction("SecureLogin", "Secure");
                    }
                }

                else if (userAccount != null) 
                {
                    userRequest = await _authorizeService.LoginUserAsync(loginUser, allowLockout: true);

                    if (userRequest.Succeeded) 
                    {
                        userAccount.IsActive = true;

                        await _mediator.Send(new UserActionCommand()
                        {
                            userId = userAccount.Id,
                            Action = "Login Success"
                        });

                        return LocalRedirect(returnUrl);
                    }
                    else if (userRequest.IsLockedOut)
                    {
                        return RedirectToAction("LockoutUser", "Confirm");     
                    } 
                    else
                    {
                        ApplicationUser userVisitor = await _signInManager.UserManager.FindByEmailAsync(loginUser.Email);

                        int failedAttempts = await _signInManager.UserManager.GetAccessFailedCountAsync(userVisitor);
                        int totalAttempts = _signInManager.Options.Lockout.MaxFailedAccessAttempts;
                        string alertMessage = $"Login has failed. Remaining attempts {failedAttempts} of {totalAttempts}";

                        ModelState.AddModelError(string.Empty, alertMessage);

                        await _mediator.Send(new UserActionCommand()
                        {
                            userId = userAccount.Id,
                            Action = "Login Failed"
                        });

                        _notificationService.ErrorMessage(
                            "Invalid Login attempt! Check your email or password and try again"
                        );

                        return View(loginUser);
                    }

                }
                else
                {
                    _notificationService.ErrorMessage(
                        "Unable to find valid user account! Check your credentials and try again"
                    );
                }
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
