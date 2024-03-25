using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Client.Interfaces;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class ConfirmController : RootController<ConfirmController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _authorizeService;

        public ConfirmController(
            UserManager<ApplicationUser> userManager, IAuthorizeService authorizeService)
        {
            _userManager = userManager;
            _authorizeService = authorizeService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirmation(string emailAccount)
        {
            var confirmUser = new ConfirmUserDto
            {
                EmailConfirmationUrl = null
            };

            if (emailAccount != null)
            {
                var currentAccount = await _userManager.FindByEmailAsync(emailAccount);

                if (currentAccount != null)
                {
                    var accountId = await _userManager.GetUserIdAsync(currentAccount);
                    var confirmationLink = await _authorizeService.SendConfirmationCodeAsync(currentAccount);

                    confirmUser.EmailConfirmationUrl = Url.Action(
                        "ConfirmUser",
                        "Confirm",
                        new
                        {
                            accountId,
                            confirmationLink
                        }
                    );
                } else
                {
                    _notificationService.ErrorMessage($"The following account could not be found");
                }
            }

            return View(confirmUser);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmUser(string accountId, string confirmationLink)
        {
            if (accountId == null || confirmationLink == null)
            {
                return RedirectToAction("Register", "Auth");
            }

            var existingAccount = await _userManager.FindByIdAsync(accountId);
            var validateAccount = await _authorizeService.ConfirmUserAsync(
                existingAccount, confirmationLink
            );

            if (!validateAccount.Succeeded)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LockoutUser()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordConfirmation(string emailAccount)
        {
            var userAccount = new RequestUserDto
            {
                Email = emailAccount
            };
            
            return View(userAccount);
        }
    }
}
