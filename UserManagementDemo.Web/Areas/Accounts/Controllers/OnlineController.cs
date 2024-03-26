using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Service.Client.Interfaces;
using UserManagementDemo.Web.Controllers;

namespace UserManagementDemo.Web.Areas.Accounts.Controllers
{
    [Area("Accounts")]
    public class OnlineController : RootController<OnlineController>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOnlineUserService _onlineManager;

        public OnlineController(
            UserManager<ApplicationUser> userManager, IOnlineUserService onlineManager)
        {
            _userManager = userManager;
            _onlineManager = onlineManager;
        }

        public IEnumerable<ApplicationUser> UserAccounts { get; set; }

        [HttpGet]
        public async Task<PartialViewResult> Index()
        {
            UserAccounts = await _userManager.Users.ToListAsync();
            var onlineUsers = await _onlineManager.GetOnlineUsers(UserAccounts);

            return new PartialViewResult
            {
                ViewName = "_ListOnlineAccounts",
                ViewData = new ViewDataDictionary<IList<AccountUser>>(
                    ViewData, onlineUsers),
            };
        }

        [HttpPost]
        public async Task<IActionResult> OnlineAccess(string userId)
        {
            ApplicationUser userAccount = await _userManager.Users.FirstOrDefaultAsync(
                user => user.Id == userId);

            if (userAccount != null)
            {
                if (userAccount.LockoutEnd != null || userAccount.LockoutEnd > DateTime.Now)
                {
                    userAccount.LockoutEnd = null;
                    await _userManager.UpdateAsync(userAccount);

                    _notificationService.SuccessMessage(
                        $"Online account {userAccount.UserName} access has been granted");
                }
                else
                {
                    userAccount.LockoutEnd = DateTime.Now.AddMonths(3);
                    await _userManager.UpdateAsync(userAccount);

                    _notificationService.ErrorMessage($"" +
                        $"Online account {userAccount.UserName} access has been revoked");
                }
            }
            
            return RedirectToAction("Index");
        }
    }
}
