using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Infrastructure.Utilities;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.Service.Client.Entities
{
    public class AuthenticateService : IAuthenticateService 
    {
        private const string AuthenticatorURIFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly SecureToken _jwtSettings;

        public AuthenticateService(UserManager<ApplicationUser> userManager, 
            UrlEncoder urlEncoder, IOptions<SecureToken> jwtSettings)
        {
            _userManager = userManager;
            _urlEncoder = urlEncoder;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtSecurityToken> SetAuthTokenAsync(ApplicationUser userAccount)
        {
            var userClaims = await _userManager.GetClaimsAsync(userAccount);
            var userRoles = await _userManager.GetRolesAsync(userAccount);
            var roleClaims = new List<Claim>();

            foreach (var role in userRoles) 
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var userData = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userAccount.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userAccount.Email),
                new Claim("uid", userAccount.Id),
                new Claim("first_name", userAccount.FirstName),
                new Claim("last_name", userAccount.LastName)
            }
            .Union(userClaims).Union(roleClaims);

            return JWTGeneration(userData);
        }

        public async Task<string> GetAuthKeyAsync(ApplicationUser userAccount)
        {
            var currentAuthKey = await _userManager.GetAuthenticatorKeyAsync(userAccount);

            if (string.IsNullOrEmpty(currentAuthKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(userAccount);
                currentAuthKey = await _userManager.GetAuthenticatorKeyAsync(userAccount);
            }

            var generatedAuthKey = WebFormatUtility.FormatStringAsCode(currentAuthKey);
            return generatedAuthKey;
        }

        public string GenerateQRCodeUri(string email, string authToken)
        {
            return string.Format(
                AuthenticatorURIFormat, 
                _urlEncoder.Encode("Microsoft.AspNetCore.Identity.UI"),
                _urlEncoder.Encode(email), 
                authToken
           );
        }

        private JwtSecurityToken JWTGeneration(IEnumerable<Claim> authClaims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signCredentials
           );

            return jwtSecurityToken;
        }
    }
}
