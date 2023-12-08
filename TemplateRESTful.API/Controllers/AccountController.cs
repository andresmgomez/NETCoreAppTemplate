using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Entities.Models.Users;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Service.Common.Account;

namespace TemplateRESTful.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _manageAccount;
 
        public AccountController(
            UserManager<ApplicationUser> userManager,
            IAuthorizeService manageAccount)
        {
            _userManager = userManager;
            _manageAccount = manageAccount;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAccount(RegisterUser onlineUser)
        {
            var existingAccount = await _userManager.FindByEmailAsync(onlineUser.Email);

            if (existingAccount != null)
            {
                return NotFound(ServerResponse<string>.FailedMessage(
                    message: $"The following email account {onlineUser.Email} is already in our system")
                );
            }

            var registerAccount = await _manageAccount.RegisterAccountAsync(onlineUser);

            if (registerAccount.Succeeded)
            {
                return Ok(ServerResponse<string>.SuccessMessage(
                    message: $"The following email account {onlineUser.Email} registered successfully")
                );
            }

            return BadRequest(ServerResponse<string>.FailedMessage(
                message: $"There was a problem trying to register {onlineUser.Email} email account")
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount(LoginUser onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            if (userAccount == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following email account {onlineUser.Email} is invalid.")
                );
            }

            var checkCredentials = await _userManager.CheckPasswordAsync(userAccount, onlineUser.Password);

            if (checkCredentials == false)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The entered credentials does not match our records")
                );
            }

            var result = await _manageAccount.LoginAccountAsync(onlineUser, allowLockout: true);

            if (result.Succeeded) 
            {
                return Ok(ServerResponse<string>.SuccessMessage(
                     message: $"The following email account {onlineUser.Email} logged in successfully")
                 );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
               message: $"There was a problem trying to login {onlineUser.Email} email account")
           );
        }

        [HttpGet("confirm-account")]
        public async Task<IActionResult> ConfirmAccount([FromQuery] RequestUser onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            var confirmationCode = await _manageAccount.SendConfirmationCodeAsync(userAccount);

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
            var confirmAccount = await _manageAccount.ConfirmAccountAsync(userAccount, code);

            if (userAccount.EmailConfirmed ==  true)
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

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] RequestUser onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            if (userAccount == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following email account {onlineUser.Email} is invalid.")
                );
            }

            var accountResetToken = await _manageAccount.SendPasswordResetTokenAsync(userAccount);

            return Ok(ServerResponse<object>.SuccessMessage(
               accountResetToken,
               message: $"Copy the following generated code to reset your credentials")
           );
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] ForgotUser onlineUser)
        {
            var resetToken = onlineUser.Code;
            var userAccount = await _userManager.FindByEmailAsync(onlineUser.Email);

            if (resetToken == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"A valid generated token is needed to reset your credentials")
                );
            }

            var result = await _manageAccount.ResetPasswordAccountAsync(userAccount, resetToken, onlineUser.Password);

            if (result.Succeeded)
            {
                return Ok(ServerResponse<string>.SuccessMessage(
                      message: $"The following email account {onlineUser.Email} credentials have been changed")
                );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
                message: $"There has been a problem trying to reset your credentials")
            );
        }

        private string ObtainIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
