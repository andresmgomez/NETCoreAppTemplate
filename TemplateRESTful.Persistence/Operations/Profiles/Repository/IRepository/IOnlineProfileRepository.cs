using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities.Profiles;

namespace TemplateRESTful.Persistence.Operations.Profiles
{
    public interface IOnlineProfileRepository
    {
        IQueryable<OnlineProfile> OnlineProfiles { get; }
        Task<IReadOnlyList<OnlineProfile>> OnlineProfilesAsync();
        Task<OnlineProfile> GetSingleProfile(int userId);
        Task CreateOnlineProfile(OnlineProfile onlineProfile);
        Task UpdateOnlineProfile(OnlineProfile onlineProfile);
    }
}
