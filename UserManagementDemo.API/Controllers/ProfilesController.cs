using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOnlineProfileService _profileManager;

        public ProfilesController(
            UserManager<ApplicationUser> userManager, IOnlineProfileService profileManager)
        {
            _userManager = userManager;
            _profileManager = profileManager;
        }

        [HttpGet("profiles")]
        public async Task<IActionResult> AccountProfiles()
        {
            var onlineUsers = await _userManager.Users.ToListAsync();
            var onlineProfiles = await _profileManager.Entities.OrderByDescending(p => p.Id).ToListAsync();
           
            var accountProfiles = _profileManager.GetOnlineProfiles(onlineUsers, onlineProfiles);

            if (accountProfiles.Any())
            {
                return Ok(ServerResponse<object>.SuccessMessage(
                    apiResponse: accountProfiles
                ));
            }

            return NotFound(ServerResponse<string>.FailedMessage(
                message: $"An error has occurred. Unable to fetch online profiles")
            );
        }

        [HttpGet("profiles/single-profile")]
        public async Task<IActionResult> AccountProfile([FromQuery] RequestUserDto onlineUser)
        {

            var userProfiles = await _profileManager.Entities.OrderByDescending(p => p.Id).ToListAsync();

            var userProfile = userProfiles.Find(p => p.EmailAddress.Equals(onlineUser.Email));
            var userAccount = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userProfile.EmailAddress);

            if (onlineUser.Email == null) 
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"An error has occurred. You need to provide a valid account")
                 );
            }

            var profileAccount = _profileManager.GetOnlineProfile(userAccount, userProfile);


            if (profileAccount != null)
            {
                return Ok(ServerResponse<object>.SuccessMessage(
                   apiResponse: profileAccount
                ));
            }

            return NotFound(ServerResponse<string>.FailedMessage(
                message: $"An error has occurred. Unable to fetch online profile")
            );
        }
    }
}
