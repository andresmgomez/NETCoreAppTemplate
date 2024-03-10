using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Data.Profiles.Commands;
using TemplateRESTful.Persistence.Data.Profiles.Queries;
using TemplateRESTful.Web.Areas.Accounts.Models;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class ProfilesController : RootController<ProfilesController>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilesController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(int profileId = 0)
        {
            var onlineUser = await _userManager.GetUserAsync(User);
            var onlineProfile = LoadUserProfile(onlineUser);

            if (profileId != 0) 
            {
                var existingProfile = await _mediator.Send(
                    new GetOnlineProfileQuery()
                    {
                        Id = profileId,
                    }
                );

                if (existingProfile.IsSuccessful)
                {
                    var userProfile = _mapper.Map<OnlineProfile>(existingProfile.ApiResponse);
                    var existingModel = new UserProfileViewModel
                    {
                        OnlineUser = onlineUser,
                        OnlineProfile = userProfile
                    };

                    return View(existingModel);
                }
            }
            else
            {
                var profileModel = new UserProfileViewModel
                {
                    OnlineUser = onlineUser,
                    OnlineProfile = onlineProfile
                };

                return View(profileModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(int profileId,
            UserProfileViewModel profileModel, string returnUrl = null)
        {
            var onlineProfile = profileModel.OnlineProfile;
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (profileId > 0)
                {
                    onlineProfile.Id = profileId;
                    var updateProfile = _mapper.Map<UpdateProfileCommand>(onlineProfile);
                    await _mediator.Send(updateProfile);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    var createProfile = _mapper.Map<CreateProfileCommand>(onlineProfile);
                    await _mediator.Send(createProfile);

                    return LocalRedirect(returnUrl);
                }
            }

            return View(profileModel);
        }

        private OnlineProfile LoadUserProfile(ApplicationUser onlineUser)
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
