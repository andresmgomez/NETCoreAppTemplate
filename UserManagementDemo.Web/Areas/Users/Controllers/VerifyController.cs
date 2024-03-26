using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Service.Client.Interfaces;
using UserManagementDemo.Web.Controllers;

namespace UserManagementDemo.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class VerifyController : RootController<VerifyController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticateService _authenticateManager;
        private readonly IOnlineUserService _onlineUserManager;

        public VerifyController(UserManager<ApplicationUser> userManager,
            IAuthenticateService authenticateManager, IOnlineUserService onlineUserManager)
        {
            _userManager = userManager;
            _authenticateManager = authenticateManager;
            _onlineUserManager = onlineUserManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TwoFactorVerification()
        {
            var userAccount = await _userManager.GetUserAsync(User);
            var userEmail = await _userManager.GetEmailAsync(userAccount);

            if (userAccount != null)
            {
                var userToken = await _authenticateManager.GetAuthKeyAsync(userAccount);
                var userQrCode = _authenticateManager.GenerateQRCodeUri(userEmail, userToken);

                var secureUser = new ProtectUserDto
                {
                    Token = userToken,
                    QRCode = userQrCode
                };
                
                return View(secureUser);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TwoFactorVerification(ProtectUserDto secureAccess, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            var userAccount = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var tokenProvider = _userManager.Options.Tokens.AuthenticatorTokenProvider;
                var verificationCode = secureAccess.Code.Replace(" ", string.Empty);

                var validToken = await _userManager.VerifyTwoFactorTokenAsync(
                    userAccount, tokenProvider, verificationCode);

                if (validToken)
                {
                    await _userManager.SetTwoFactorEnabledAsync(userAccount, true);

                    _notificationService.SuccessMessage(
                        "You have successfully enabled 2FA in your account");

                    return LocalRedirect(returnUrl);
                }
            }

            return View(secureAccess);
        }

        [HttpGet]
        [Authorize]
        public IActionResult VerifyContactNumber()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> VerifyContactNumber(
            VerifyUserDto requestUser, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var currentUser =  await _userManager.GetUserAsync(User);

            if (ModelState.IsValid) 
            {
                if (currentUser != null)
                {
                    var changeContact = await _onlineUserManager.ChangeContactInfo(
                        currentUser, requestUser);

                    if (!changeContact.Succeeded)
                    {
                        _notificationService.ErrorMessage(
                            "There is an error trying to change your contact number");

                        return View();
                    }

                    _notificationService.SuccessMessage(
                        "Your contact number has been changed");

                    return LocalRedirect(returnUrl);
                }
            }

            return View(requestUser);
        }
    }
}
