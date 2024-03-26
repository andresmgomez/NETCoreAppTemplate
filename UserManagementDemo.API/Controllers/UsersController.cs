using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 
        
        private readonly IAuthorizeService _authorizeService;
        private readonly IAuthenticateService _authenticateService;

        public UsersController(
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAuthorizeService authorize, IAuthenticateService authenticate)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizeService = authorize;
            _authenticateService = authenticate;
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

            var registerAccount = await _authorizeService.RegisterUserAsync(onlineUser);

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
                    message: $"The credentials you have entered doesn't match our records")
                );
            }

            var result = await _authorizeService.LoginUserAsync(onlineUser, allowLockout: true);


            if (result.Succeeded) 
            {
                userAccount.IsActive = true;
                var createToken = await _authenticateService.SetAuthTokenAsync(userAccount);
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

        [HttpGet("logout")]
        public async Task<IActionResult> LogoutUser([FromQuery] RequestUserDto onlineUser)
        {
            var userAccount = await _userManager.FindByEmailAsync(onlineUser.Email);

            if (userAccount == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"An error has occurred. You need to provide a valid account")
                 );
            }

            var currentSession = await _userManager.GetUserIdAsync(userAccount);

            if (currentSession != null)
            {
                await _signInManager.SignOutAsync();

                return Ok(ServerResponse<string>.SuccessMessage(
                    message: $"The following email account {onlineUser.Email} logged out successfully")
                );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
               message: $"There was a problem trying to logout {onlineUser.Email} email account")
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

            var accountResetToken = await _authorizeService.SendPasswordResetTokenAsync(userAccount);

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

            var result = await _authorizeService.ResetUserPasswordAsync(userAccount, resetToken, onlineUser.Password);

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
