using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Service.Client.Interfaces;
using UserManagementDemo.Web.Areas.Accounts.Models;

namespace UserManagementDemo.Web.Views.Shared.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOnlineProfileService _profileManager;

        public HeaderViewComponent(
            UserManager<ApplicationUser> userManager, IOnlineProfileService profileManager)
        {
            _userManager = userManager;
            _profileManager = profileManager;    
        }

        public UserProfileViewModel UserProfile { get; set; }
        public OnlineProfile OnlineProfile { get; set; }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            OnlineProfile = await _profileManager.Entities.FirstOrDefaultAsync(
                p => p.EmailAddress == currentUser.Email);


            if (OnlineProfile != null)
            {
                UserProfile = new UserProfileViewModel
                {
                    OnlineProfile = OnlineProfile
                };

                return View("Default", UserProfile);
            }

            UserProfile = new UserProfileViewModel
            {
                OnlineProfile = new OnlineProfile()
            };

            return View("Default", UserProfile);
        }
    }
}
