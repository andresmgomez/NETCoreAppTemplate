using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Service.Client.Interfaces;
using TemplateRESTful.Service.Server.Interfaces;
using TemplateRESTful.Web.Controllers;

namespace TemplateRESTful.Web.Areas.Users.Controllers
{
    [Area("Users")]
    public class ResetController : RootController<ResetController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizeService _authorizeService;
        private readonly IEmailService _emailService;

        public ResetController(
            UserManager<ApplicationUser> userManager, IAuthorizeService authorizeService, 
            IEmailService emailService)
        {
            _userManager = userManager;
            _authorizeService = authorizeService;
            _emailService = emailService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotCredentials()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotCredentials(RequestUserDto requestUser)
        {
            if (ModelState.IsValid) 
            {
                var existingAccount = await _userManager.FindByEmailAsync(requestUser.Email);

                if (existingAccount != null) 
                {
                    var generateResetToken = await _authorizeService.SendPasswordResetTokenAsync(existingAccount);

                    var forgotCredentialsLink = Url.Action(
                        "ResetCredentials",
                        "Reset", new
                        {
                            resetLink = generateResetToken,
                            emailAccount = requestUser.Email
                        },
                        protocol: Request.Scheme
                    );

                    var sendMessage = new EmailMessage(new string[]
                    {
                        requestUser.Email
                    },
                    "RESTful API - Instructions How To Reset Your Password",
                    $@"<h4>Hello, {requestUser.Email}</h4>,
                       <p>Someone has requested a link to change your password. Click on the link below
                       to reset your credentials.</p>
                       <h4><a href='{HtmlEncoder.Default.Encode(forgotCredentialsLink)}'>Change my password</a></h4>
                       <p>If you haven't requested this, just ignore this email, and report to your network administrator.</p>"
                    );

                    await _emailService.SendEmailAsync(sendMessage);

                    return RedirectToAction(
                        "PasswordConfirmation",
                        "Confirm", new
                        {
                            emailAccount = requestUser.Email,
                        }
                    );
                }
                else
                {
                    _notificationService.WarningMessage($"You need to provide a valid email account");
                }
            }

            return View(requestUser);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetCredentials(string resetLink, string emailAccount)
        {
            if (resetLink != null && emailAccount != null)
            {
                var resetCredentials = new ForgotUserDto()
                {
                    Code = resetLink,
                    Email = emailAccount,
                };

                return View(resetCredentials);
            }

            _notificationService.WarningMessage($"An invalid link was provided to reset credentials");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetCredentials(ForgotUserDto forgotUser)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = await _userManager.FindByEmailAsync(forgotUser.Email);

                if (existingAccount != null) 
                {
                    IdentityResult changeCredentials = await _authorizeService.ResetUserPasswordAsync(
                        existingAccount, forgotUser.Code, forgotUser.Password);

                    if (changeCredentials.Succeeded)
                    {
                        _notificationService.SuccessMessage($"Your credentials have been changed successfully");

                        return RedirectToAction("Login", "Auth");
                    }

                    _notificationService.ErrorMessage("There has been a problem trying to reset your credentials");
                }

                return View();
            }

            return View(forgotUser);
        }

    }
}
