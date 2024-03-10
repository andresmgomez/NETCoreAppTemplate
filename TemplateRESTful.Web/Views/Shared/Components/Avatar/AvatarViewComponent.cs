using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Operations.Profiles;

namespace TemplateRESTful.Web.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly IOnlineProfileManager _profileManager;

        public AvatarViewComponent(IOnlineProfileManager profileManager)
        {
            _profileManager = profileManager;
        }   

        public OnlineProfile OnlineProfile { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var useProfile = await _profileManager.Entities.FirstOrDefaultAsync();

            if (useProfile != null)
            {
                var onlineProfile = new OnlineProfile
                {
                    Picture = useProfile.Picture
                };

                return View("Default", onlineProfile);
            }

            var emptyProfile = new OnlineProfile
            {
                Picture = null
            };

            return View("Default", emptyProfile);            
        }
    }
}
