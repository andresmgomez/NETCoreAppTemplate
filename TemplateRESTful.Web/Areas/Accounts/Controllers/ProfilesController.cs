using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Client.Actions.Commands;
using TemplateRESTful.Service.Client.Actions.Queries;
using TemplateRESTful.Service.Client.Interfaces;
using TemplateRESTful.Web.Areas.Accounts.Models;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class ProfilesController : RootController<ProfilesController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOnlineProfileService _profileManager;

        public ProfilesController(UserManager<ApplicationUser> userManager,
            IOnlineProfileService profileManager)
        {
            _userManager = userManager;
            _profileManager = profileManager;
        }

        [HttpGet]
        public async Task<IActionResult> SaveProfile(string profileId = null)
        {
            var onlineUser = await _userManager.GetUserAsync(User);
            var onlineProfile = _profileManager.SetOnlineProfile(onlineUser);

            if (profileId != null) 
            {
                var existingProfile = await _mediator.Send(
                    new GetOnlineProfileQuery()
                    {
                        Id = profileId,
                        EmailAddress = onlineProfile.EmailAddress
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
        public async Task<IActionResult> SaveProfile(string profileId,
            UserProfileViewModel profileModel, string returnUrl = null)
        {
            var currentProfile = await _profileManager.GetByIdAsync(profileId);
            var onlineProfile = profileModel.OnlineProfile;

            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (profileId != null)
                {
                    onlineProfile.Id = profileId;

                    if (Request.Form.Files.Count > 0)
                    {
                        await ChangeProfilePicture(onlineProfile);
                    }
                    else
                    {
                        onlineProfile.Picture = currentProfile.Picture;
                    }

                    var updateProfile = _mapper.Map<UpdateProfileCommand>(onlineProfile);
                    await _mediator.Send(updateProfile);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    if (Request.Form.Files.Count > 0)
                    {
                        await ChangeProfilePicture(onlineProfile);
                    }

                    var createProfile = _mapper.Map<CreateProfileCommand>(onlineProfile);
                    await _mediator.Send(createProfile);

                    return LocalRedirect(returnUrl);
                }
            }

            return View(profileModel);
        }

        private async Task<byte[]> ChangeProfilePicture(OnlineProfile profile)
        {
            if (Request.Form.Files.Count > 0)
            {
                IFormFile picture = Request.Form.Files.FirstOrDefault();

                using (var dataStream = new MemoryStream())
                {
                    await picture.CopyToAsync(dataStream);
                    profile.Picture = dataStream.ToArray();
                }
            }

            return default;
        }
    }
}
