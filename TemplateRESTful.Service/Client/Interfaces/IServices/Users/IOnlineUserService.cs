using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Service.Client.Interfaces
{
    public interface IOnlineUserService
    {
        Task<IEnumerable<AccountUser>> GetOnlineUsers(IEnumerable<ApplicationUser> identityUsers);
        Task<List<string>> GetOnlineUserRoles(ApplicationUser user);
        Task<IdentityResult> ChangeContactInfo(ApplicationUser identityUser, VerifyUserDto verifyUser);
    }
}
