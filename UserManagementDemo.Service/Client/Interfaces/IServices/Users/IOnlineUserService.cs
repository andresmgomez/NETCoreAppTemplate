using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Service.Client.Interfaces
{
    public interface IOnlineUserService
    {
        Task<IEnumerable<AccountUser>> GetOnlineUsers(IEnumerable<ApplicationUser> identityUsers);
        Task<List<string>> GetOnlineUserRoles(ApplicationUser user);
        Task<IdentityResult> ChangeContactInfo(ApplicationUser identityUser, VerifyUserDto verifyUser);
    }
}
