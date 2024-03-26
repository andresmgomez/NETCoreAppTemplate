using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _authorizeService;
        private readonly IOnlineUserService _userService;

        public AccountsController(UserManager<ApplicationUser> userManager, 
            IAuthorizeService authorizeService, IOnlineUserService userService)
        {
            _userManager = userManager;
            _authorizeService = authorizeService;
            _userService = userService;
        }

        [HttpGet("confirm-account")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] RequestUserDto onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            var confirmationCode = await _authorizeService.SendConfirmationCodeAsync(userAccount);

            if (userAccount == null || confirmationCode == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"An error has occurred. One of the fields entered is invalid")
                 );
            }

            return Ok(ServerResponse<object>.SuccessMessage(
               confirmationCode,
               message: $"Copy the following generated code for account confirmation")
           );
        }

        [HttpPost("confirm-account")]
        public async Task<IActionResult> ConfirmAccount(string email, string code)
        {
            var userAccount = await _userManager.FindByEmailAsync(email);
            var confirmAccount = await _authorizeService.ConfirmUserAsync(userAccount, code);

            if (userAccount.EmailConfirmed == true)
            {
                return Accepted(ServerResponse<string>.SuccessMessage(
                   message: $"The following email account has already been confirmed"
                   ));
            }

            if (confirmAccount.Succeeded)
            {
                return Ok(ServerResponse<string>.SuccessMessage(
                    message: $"The following email account {userAccount.Email} has been confirmed"
                    ));
            }

            return BadRequest(ServerResponse<string>.FailedMessage(
                   message: $"There was a problem trying to confirm {userAccount.Email} email account")
            );
        }

        [HttpPost("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromBody] VerifyUserDto onlineUser, string emailAddress)
        {
            var userAccount = await _userManager.FindByEmailAsync(emailAddress);

            if (userAccount != null) 
            {
                if (userAccount.PhoneNumber != null)
                {
                    return Accepted(ServerResponse<string>.SuccessMessage(
                       message: $"The following email account {userAccount.Email} has already been verified"
                       ));
                }
                
                var verifyAccount = await _userService.ChangeContactInfo(userAccount, onlineUser);

                if (verifyAccount.Succeeded)
                {
                    return Ok(ServerResponse<string>.SuccessMessage(
                        message: $"The following email account {userAccount.Email} has been verified"
                        ));
                }
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
                   message: $"There was a problem trying to verify {userAccount.Email} email account")
            );
        }
    }
}
