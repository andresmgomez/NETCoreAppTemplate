using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Persistence.Data.Interfaces;

namespace UserManagementDemo.Service.Client.Interfaces
{
    public interface IOnlineProfileService : IGenericRepositoryAsync<OnlineProfile> 
    {
        IEnumerable<object> GetOnlineProfiles(IEnumerable<ApplicationUser> onlineUsers,
            IEnumerable<OnlineProfile> onlineProfiles);

        OnlineProfileDto GetOnlineProfile(ApplicationUser onlineUser, OnlineProfile onlineProfile);

        OnlineProfile SetOnlineProfile(ApplicationUser onlineUser);
    }
}
