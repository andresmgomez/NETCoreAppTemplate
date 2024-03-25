using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.API.Controllers
{
    [Route("api/accounts/admins")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthorizeService _authorizeService;
        private readonly IAuthenticateService _authenticateService;

        public AdminsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAuthorizeService authorizeService, IAuthenticateService authenticateService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorizeService = authorizeService;
            _authenticateService = authenticateService;
        }

        [HttpPost("send-access")]
        public async Task<IActionResult> AuthorizeAdminAccount([FromQuery] LoginUserDto adminUser)
        {
            var userAccount = await _userManager.FindByEmailAsync(adminUser.Email);
            var adminAccount = await _userManager.IsInRoleAsync(userAccount, "Administrator");

            if (userAccount == null)
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following account you have provided is invalid.")
                );
            }

            if (userAccount != null && adminAccount)
            {
                var loginAccount = await _authorizeService.LoginUserAsync(adminUser, false);

                if (loginAccount.RequiresTwoFactor)
                {
                    var checkCredentials = await _userManager.CheckPasswordAsync(userAccount, adminUser.Password);

                    if (checkCredentials ==  false)
                    {
                        return BadRequest(ServerResponse<string>.FailedMessage(
                            message: $"The credentials you have entered are not valid.")
                        );
                    }

                    var generateAccessCode = await _userManager.GenerateTwoFactorTokenAsync(userAccount, "Email");

                    var accountResponse = new
                    {
                        authorizedAccount = adminAccount,
                        authorizedCode = generateAccessCode,
                    };

                    return Ok(ServerResponse<object>.SuccessMessage(
                        apiResponse: accountResponse,
                        message: $"Copy the following authorization code. This will expire in a few minutes.")
                    );
                }

                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following account doesn't have 2fa enabled.")
                );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
              message: $"This account doesn't have permission to login as Administrator")
            );
        }

        [HttpPost("auth-access")]
        public async Task<IActionResult> AuthenticateAdminAccount([FromQuery] AdminUserDto secureUser)
        {
            
            if (string.IsNullOrEmpty(secureUser.AccessCode))
            {
                return BadRequest(ServerResponse<string>.FailedMessage(
                    message: $"The following access code provided can't empty.")
                );
            }

            var verifyAdminAccess = await _signInManager.TwoFactorSignInAsync("Email",
                secureUser.AccessCode, secureUser.KeepSession, secureUser.RememberBrowser
            );

            if (verifyAdminAccess.Succeeded)
            {
                var userAccount = await _userManager.FindByEmailAsync(secureUser.EmailAccount);
                var createToken = await _authenticateService.SetAuthTokenAsync(userAccount);
                var authToken = new JwtSecurityTokenHandler().WriteToken(createToken);

                return Ok(ServerResponse<object>.SuccessMessage(
                    apiResponse: authToken,
                    message: $"The following admin account logged in successfully.")
                );
            }

            return Unauthorized(ServerResponse<string>.FailedMessage(
                message: $"This following admin account couldn't be authenticated.")
            );
        }
    }
}
