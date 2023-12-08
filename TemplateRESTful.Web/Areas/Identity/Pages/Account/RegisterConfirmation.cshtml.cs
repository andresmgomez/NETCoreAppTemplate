using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Service.Common.Account;
using TemplateRESTful.Web.Implementation;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : ImplementService<RegisterConfirmationModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _manageAccount;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager,
            IAuthorizeService manageAccount,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _manageAccount = manageAccount;
            _logger = logger;
        }

        public string Email { get; set; }
        public bool ConfirmAccountLink { get; set; } = true;
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return RedirectToPage("/Register");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                _logger.LogInformation("User try to register an existing account");
                _notificationService.ErrorMessage($"Unable to find account with giving email address");
            }

            Email = email;

            if (ConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _manageAccount.SendConfirmationCodeAsync(user);

                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { 
                        area = "Identity", 
                        userId, 
                        code
                    },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
