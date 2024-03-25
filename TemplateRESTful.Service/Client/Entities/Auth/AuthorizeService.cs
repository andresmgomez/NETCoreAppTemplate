using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Enums.Account;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.Service.Client.Entities
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthorizeService(IMapper mapper, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUser)
        {
            var userAccount = _mapper.Map<ApplicationUser>(registerUser); 
            var createAccount = await _userManager.CreateAsync(userAccount, registerUser.Password);
            
            await _userManager.AddToRoleAsync(userAccount, UserRoles.RegularUser.ToString());

            return createAccount;
        }

        public async Task<SignInResult> LoginUserAsync(LoginUserDto loginUser, bool allowLockout)
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

        public async Task<IdentityResult> ConfirmUserAsync(ApplicationUser userAccount, string confirmationCode)
        {
            confirmationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmationCode));
            var confirmAccount = await _userManager.ConfirmEmailAsync(userAccount, confirmationCode);

            return confirmAccount;
        }

        public async Task<IdentityResult> ResetUserPasswordAsync(ApplicationUser userAccount, string resetToken, string userPassword)
        {
            var decodeToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetToken));
            var resetPassword = await _userManager.ResetPasswordAsync(userAccount, decodeToken, userPassword);

            return resetPassword;
        }
    }
}
