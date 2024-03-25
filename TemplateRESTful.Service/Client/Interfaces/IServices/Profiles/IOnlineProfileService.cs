using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Persistence.Data.Interfaces;

namespace TemplateRESTful.Service.Client.Interfaces
{
    public interface IOnlineProfileService : IGenericRepositoryAsync<OnlineProfile> 
    {
        IEnumerable<object> GetOnlineProfiles(IEnumerable<ApplicationUser> onlineUsers,
            IEnumerable<OnlineProfile> onlineProfiles);

        OnlineProfileDto GetOnlineProfile(ApplicationUser onlineUser, OnlineProfile onlineProfile);

        OnlineProfile SetOnlineProfile(ApplicationUser onlineUser);
    }
}
