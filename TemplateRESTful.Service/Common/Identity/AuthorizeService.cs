using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Entities.DTOs.Account;
using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Domain.Enums.Account;
using TemplateRESTful.Infrastructure.Identity;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Service.Common.Identity;
using AutoMapper;
using TemplateRESTful.Service.Features.Notifications;
using Microsoft.AspNetCore.WebUtilities;

namespace TemplateRESTful.Service.Common.Account
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizeService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAccountAsync(RegisterUser registerUser)
        {
            var userAccount = new ApplicationUser
            {
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                UserName = registerUser.Email,
                Email = registerUser.Email,
                IsActive = registerUser.AccountStatus
            };

            var createAccount = await _userManager.CreateAsync(userAccount, registerUser.Password);
            await _userManager.AddToRoleAsync(userAccount, UserRoles.AccountUser.ToString());

            return createAccount;
        }

        public async Task<SignInResult> LoginAccountAsync(LoginUser loginUser, bool allowLockout)
        {
            var authResult = await _signInManager.PasswordSignInAsync(
                loginUser.Email, loginUser.Password, loginUser.RememberMe, allowLockout
            );

            return authResult;
        }

        public async Task<string> SendConfirmationCodeAsync(ApplicationUser currentAccount)
        {
            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(currentAccount);
            confirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmationCode));

            return confirmationCode;
        }

        public async Task<string> SendPasswordResetTokenAsync(ApplicationUser currentAccount)
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(currentAccount);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            return resetToken;
        }

        public async Task<IdentityResult> ConfirmAccountAsync(ApplicationUser userAccount, string confirmationCode)
        {
            confirmationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationCode));
            var confirmAccount = await _userManager.ConfirmEmailAsync(userAccount, confirmationCode);

            return confirmAccount;
        }

        public async Task<IdentityResult> ResetPasswordAccountAsync(ApplicationUser userAccount, string resetToken, string userPassword)
        {
            var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetToken));
            var resetPassword = await _userManager.ResetPasswordAsync(userAccount, decodeToken, userPassword);

            return resetPassword;
        }
    }
}
