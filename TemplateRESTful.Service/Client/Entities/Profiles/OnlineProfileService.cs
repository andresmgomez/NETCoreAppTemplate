using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Persistence.Data.Generic;
using TemplateRESTful.Persistence.Data.Interfaces;
using TemplateRESTful.Persistence.Storage.DbContexts;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.Service.Client.Entities
{
    public class OnlineProfileService : GenericRepositoryAsync<OnlineProfile>, IOnlineProfileService
    {
        // private readonly DbSet<OnlineProfile> _onlineProfile;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepositoryAsync<OnlineProfile> _profileRepo;

        public OnlineProfileService(ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager, IGenericRepositoryAsync<OnlineProfile> profileRepo) : base(dbContext)
        {
            // _onlineProfile = dbContext.Set<OnlineProfile>();
            _userManager = userManager;
            _profileRepo = profileRepo;
        }

        public IEnumerable<object> GetOnlineProfiles(IEnumerable<ApplicationUser> onlineUsers,
            IEnumerable<OnlineProfile> onlineProfiles)
        {
            var currentProfiles = onlineProfiles.Join(
                onlineUsers, o => o.EmailAddress, u => u.Email, (profile, user) => new
            {
                profile.Id,
                user.FirstName,
                profile.MiddleName,
                user.LastName,
                profile.DayOfBirth,
                profile.Occupation,
                profile.Website
            });

            return currentProfiles;
        }

        public OnlineProfileDto GetOnlineProfile(ApplicationUser onlineUser, OnlineProfile onlineProfile)
        {
            var currentProfile = new OnlineProfileDto
            {
                FirstName = onlineUser.FirstName,
                MiddleName = onlineProfile.MiddleName,
                LastName = onlineUser.LastName,
                DayOfBirth = onlineProfile.DayOfBirth,
                Occupation = onlineProfile.Occupation,
                Website = onlineProfile.Website
            };

            return currentProfile;
        }


        public OnlineProfile SetOnlineProfile(ApplicationUser onlineUser)
        {
            var profileUser = new OnlineProfileDto();
            var profileModel = new OnlineProfile
            {
                FirstName = onlineUser.FirstName,
                MiddleName = profileUser.MiddleName,
                LastName = onlineUser.LastName,
                PhoneNumber = onlineUser.PhoneNumber,
                EmailAddress = onlineUser.Email,
                DayOfBirth = profileUser.DayOfBirth,
                Occupation = profileUser.Occupation,
                Website = profileUser.Website,
                Location = profileUser.Location,
                Language = profileUser.Language
            };

            return profileModel;
        }
    }
}
