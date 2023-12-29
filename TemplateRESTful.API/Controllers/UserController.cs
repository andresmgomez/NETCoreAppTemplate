using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Service.Common.Account;
using TemplateRESTful.Service.Common.Identity;


namespace TemplateRESTful.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _authorize;
        private readonly IAuthenticateService _authenticate;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IAuthorizeService authorize,
            IAuthenticateService authenticate)
        {
            _userManager = userManager;
            _authorize = authorize;
            _authenticate = authenticate;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto onlineUser)
        {
            var existingAccount = await _userManager.FindByEmailAsync(onlineUser.Email);

            if (existingAccount != null)
            {
                return NotFound(ServerResponse<string>.FailedMessage(
                    message: $"The following email account {onlineUser.Email} is already in our system")
                );
            }

            var registerAccount = await _authorize.RegisterUserAsync(onlineUser);

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
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto onlineUser)
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

            var result = await _authorize.LoginUserAsync(onlineUser, allowLockout: true);

            if (result.Succeeded) 
            {
                userAccount.IsActive = true;
                var createToken = await _authenticate.SetAuthTokenAsync(userAccount);
                var authToken = new JwtSecurityTokenHandler().WriteToken(createToken);
          
                return Ok(ServerResponse<object>.SuccessMessage(
                    apiResponse: authToken,
                    message: $"The following email account {onlineUser.Email} logged in successfully")
                 );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
               message: $"There was a problem trying to login {onlineUser.Email} email account")
           );
        }

        [HttpGet("confirm-account")]
        public async Task<IActionResult> ConfirmUser([FromQuery] RequestUserDto onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            var confirmationCode = await _authorize.SendConfirmationCodeAsync(userAccount);

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
        public async Task<IActionResult> ConfirmUser(string email, string code)
        {
            var userAccount = await _userManager.FindByEmailAsync(email);
            var confirmAccount = await _authorize.ConfirmUserAsync(userAccount, code);

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
        public async Task<IActionResult> ResetUserPassword([FromQuery] RequestUserDto onlineUser)
        {
            var emailAccount = onlineUser.Email;
            var userAccount = await _userManager.FindByEmailAsync(emailAccount);

            if (userAccount == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following email account {onlineUser.Email} is invalid.")
                );
            }

            var accountResetToken = await _authorize.SendPasswordResetTokenAsync(userAccount);

            return Ok(ServerResponse<object>.SuccessMessage(
               accountResetToken,
               message: $"Copy the following generated code to reset your credentials")
           );
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetUserPassword([FromQuery] ForgotUserDto onlineUser)
        {
            var resetToken = onlineUser.Code;
            var userAccount = await _userManager.FindByEmailAsync(onlineUser.Email);

            if (resetToken == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"A valid generated token is needed to reset your credentials")
                );
            }

            var result = await _authorize.ResetUserPasswordAsync(userAccount, resetToken, onlineUser.Password);

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
    }
}
