using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Data;
using TemplateRESTful.Persistence.Operations.Generic;
using TemplateRESTful.Persistence.Storage.DbContexts;

namespace TemplateRESTful.Persistence.Operations.Profiles
{
    public class OnlineProfileManager : GenericRepositoryAsync<OnlineProfile>, IOnlineProfileManager
    {
        private readonly DbSet<OnlineProfile> _onlineProfile;
        private readonly IGenericRepositoryAsync<OnlineProfile> _profileRepo;

        public OnlineProfileManager(
            ApplicationDbContext dbContext, IGenericRepositoryAsync<OnlineProfile> profileRepo) : base(dbContext)
        {
            _onlineProfile = dbContext.Set<OnlineProfile>();
            _profileRepo = profileRepo;
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
