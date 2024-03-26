using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Service.Client.Interfaces
{
    public interface IAuthenticateService {
        Task<JwtSecurityToken> SetAuthTokenAsync(ApplicationUser userAccount);
        Task<string> GetAuthKeyAsync(ApplicationUser userAccount);
        string GenerateQRCodeUri(string email, string authToken);
    }
}
