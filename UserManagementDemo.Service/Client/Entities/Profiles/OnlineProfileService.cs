using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Persistence.Data.Generic;
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Persistence.Storage.DbContexts;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.Service.Client.Entities
{
    public class OnlineProfileService : GenericRepositoryAsync<OnlineProfile>, IOnlineProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepositoryAsync<OnlineProfile> _profileRepo;

        public OnlineProfileService(ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager, IGenericRepositoryAsync<OnlineProfile> profileRepo) : base(dbContext)
        {
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
