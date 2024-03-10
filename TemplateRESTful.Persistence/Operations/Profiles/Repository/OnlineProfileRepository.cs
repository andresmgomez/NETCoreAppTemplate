using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Operations.Generic;

namespace TemplateRESTful.Persistence.Operations.Profiles
{
    public class OnlineProfileRepository : IOnlineProfileRepository
    {
        private readonly IGenericRepositoryAsync<OnlineProfile> _profileRepository;

        public OnlineProfileRepository(IGenericRepositoryAsync<OnlineProfile> profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public IQueryable<OnlineProfile> OnlineProfiles => _profileRepository.Entities;

        public async Task<IReadOnlyList<OnlineProfile>> OnlineProfilesAsync()
        {
            return await _profileRepository.GetAllAsync();
        }

        public async Task<OnlineProfile> GetSingleProfile(int userId)
        {
            return await _profileRepository.GetByIdAsync(userId);
        }

        public async Task CreateOnlineProfile(OnlineProfile onlineProfile)
        {
            await _profileRepository.CreateAsync(onlineProfile);
        }

        public async Task UpdateOnlineProfile(OnlineProfile onlineProfile)
        {
            await _profileRepository.UpdateAsync(onlineProfile);
        }
    }
}
