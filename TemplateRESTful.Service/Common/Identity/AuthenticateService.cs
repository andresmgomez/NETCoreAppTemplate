using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Common.Identity;

namespace TemplateRESTful.Service.Common.Identity
{
    public class AuthenticateService : IAuthenticateService 
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecureToken _jwtSettings;

        public AuthenticateService(
            UserManager<ApplicationUser> userManager, IOptions<SecureToken> jwtSettings)
        {
            _userManager = userManager;
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
