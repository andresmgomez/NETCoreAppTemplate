using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class SecureController : RootController<SecureController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SecureController(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SecureLogin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SecureLogin(AuthSettingsDto authSettings, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var authorizedUser = await _signInManager.GetTwoFactorAuthenticationUserAsync();

                if (authorizedUser != null)
                {
                    var secureLogin = await _signInManager.TwoFactorSignInAsync("Email",
                        authSettings.TwoFactorAuthentication, authSettings.KeepSession, authSettings.RememberBrowser
                    );

                    if (secureLogin.Succeeded)
                    {
                        return LocalRedirect(returnUrl);
                    }

                    ModelState.AddModelError(string.Empty, "You need to pass a valid Authenticator Code");
                }
                else
                {
                    _notificationService.ErrorMessage("There was a problem trying to verify your Identity");
                }
            }

            return View(authSettings);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalAuth(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLogin), "Secure");
            var authProperties = _signInManager.ConfigureExternalAuthenticationProperties(
                provider, redirectUrl
            );

            return Challenge(authProperties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var socialLogin = await _signInManager.GetExternalLoginInfoAsync();
           
            if (socialLogin == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var authResult = await _signInManager.ExternalLoginSignInAsync(
                socialLogin.LoginProvider, socialLogin.ProviderKey, isPersistent: true,
                bypassTwoFactor: true);

            if (authResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                ViewData["Provider"] = socialLogin.ProviderDisplayName;

                if (socialLogin.Principal.HasClaim(claim => claim.Type == ClaimTypes.Email))
                {
                    var currentEmail = new SocialUserDto
                    {
                        EmailAddress = socialLogin.Principal.FindFirstValue(ClaimTypes.Email)
                    };

                    return View(currentEmail);
                }
            }


            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(SocialUserDto socialUser, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var loginProviders = await _signInManager.GetExternalLoginInfoAsync();

                if (loginProviders == null)
                {
                    return RedirectToAction("Login", "Auth", new { returnUrl });
                }

                IdentityResult authResult;
                var userAccount = await _userManager.FindByEmailAsync(socialUser.EmailAddress);

                if (userAccount != null)
                {
                    authResult = await _userManager.AddLoginAsync(userAccount, loginProviders);

                    if (!authResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(userAccount, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                else
                {
                    socialUser.ClaimPrincipal = loginProviders.Principal;
                    var userName = loginProviders.Principal.FindFirstValue(ClaimTypes.Name).Split(' ');

                    var newAccount = new ApplicationUser
                    {
                        UserName = socialUser.EmailAddress,
                        Email = socialUser.EmailAddress,
                        FirstName = userName[0],
                        LastName = userName[1],
                    };
                    
                    authResult = await _userManager.CreateAsync(newAccount);

                    if (authResult.Succeeded)
                    {
                        authResult = await _userManager.AddLoginAsync(newAccount, loginProviders);

                        if (authResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(userAccount, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        } 
                        else
                        {
                            foreach (var error in authResult.Errors)
                            {
                                ModelState.TryAddModelError(string.Empty, error.Description);
                            }
                        }
                    }

                    _notificationService.ErrorMessage("There has been a problem with social authentication");
                }
            }

            return View(socialUser);
        }
    }
}
