using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Domain.Models.Features;
using TemplateRESTful.Service.Common.Identity;

namespace TemplateRESTful.Infrastructure.Identity
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        public AuthenticateService(
            UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtSecurityToken> GenerateSecureToken(ApplicationUser userToken, string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(userToken);
            var userRoles = await _userManager.GetRolesAsync(userToken);
            var roleClaims = new List<Claim>();

            for (int count = 0; count < userRoles.Count; count++)
            {
                roleClaims.Add(new Claim("roles", userRoles[count]));
            }

            var tokenClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userToken.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userToken.Email),
                new Claim("uid", userToken.Id),
                new Claim("first_name", userToken.FirstName),
                new Claim("last_name", userToken.LastName),
                new Claim("full_name", $"{userToken.FirstName} {userToken.LastName}"),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims).Union(roleClaims);

            return CreateJsonWebToken(tokenClaims);
        }

        public static SecureToken GenerateRefreshToken(string ipAddress)
        {
            return new SecureToken
            {
                Token = SecureTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
                CreatedIP = ipAddress
            };
        }

        private static string SecureTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[256];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private JwtSecurityToken CreateJsonWebToken(IEnumerable<Claim> claims)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }
    }
}
