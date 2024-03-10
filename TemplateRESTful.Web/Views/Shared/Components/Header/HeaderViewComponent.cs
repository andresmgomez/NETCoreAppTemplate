using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Operations.Profiles;

namespace TemplateRESTful.Web.Views.Shared.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IOnlineProfileRepository _profileRepository;

        public HeaderViewComponent(IOnlineProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;    
        }

        public IEnumerable<OnlineProfile> OnlineProfiles { get; set; }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            OnlineProfiles = await _profileRepository.OnlineProfilesAsync();

            return View("Default", OnlineProfiles);
        }
    }
}
