using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models.Account;

namespace TemplateRESTful.Service.Common.Identity
{
    public interface IAuthenticateService
    {
        Task<JwtSecurityToken> GenerateSecureToken(ApplicationUser userToken, string ipAddress);
    }
}
