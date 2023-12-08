using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Implementation;
using TemplateRESTful.Service.Common.Account;

namespace TemplateRESTful.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : ImplementService<RegisterModel>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _manageAccount;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthorizeService manageAccount,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _manageAccount = manageAccount;
            _logger = logger;
        }

        [BindProperty]
        public RegisterUser Input { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
            if (ModelState.IsValid)
            {
                var account = await _userManager.FindByEmailAsync(Input.Email);

                if (account == null)
                {
                    var result = await _manageAccount.RegisterAccountAsync(Input);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User register a new account successfully");
                        _notificationService.SuccessMessage($"You have successfully created a new Account");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    return LocalRedirect(returnUrl);
                }

                _logger.LogInformation("User try to register an existing account");
                _notificationService.ErrorMessage($"The following account is already in our system");
            }

            return Page();
        }
    }
}