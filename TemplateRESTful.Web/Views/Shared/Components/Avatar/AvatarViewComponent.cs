using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.Web.Views.Shared.Components.Avatar
{
    public class AvatarViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOnlineProfileService _profileManager;

        public AvatarViewComponent(
            UserManager<ApplicationUser> userManager, IOnlineProfileService profileManager)
        {
            _userManager = userManager;
            _profileManager = profileManager;
        }   

        public OnlineProfile OnlineProfile { get; set; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var onlineUser = await _userManager.Users.FirstOrDefaultAsync();
            var userProfile = await _profileManager.Entities.FirstOrDefaultAsync(p => p.EmailAddress == onlineUser.Email);

            if (userProfile != null)
            {
                var onlineProfile = new OnlineProfile
                {
                    Picture = userProfile.Picture
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
