using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.Service.Client.Entities
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public OnlineUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AccountUser>> GetOnlineUsers(IEnumerable<ApplicationUser> identityUsers)
        {
            var accountUsers = new List<AccountUser>();

            foreach (var user in identityUsers)
            {
                var accountUser = new AccountUser();
                accountUser.Id = user.Id;
                accountUser.FirstName = user.FirstName;
                accountUser.LastName = user.LastName;
                accountUser.Email = user.Email;
                accountUser.EmailConfirmed = user.EmailConfirmed;
                accountUser.IsActive = user.IsActive;
                accountUser.Roles = await GetOnlineUserRoles(user);
                accountUser.LockoutEnd = user.LockoutEnd;

                accountUsers.Add(accountUser);
            }

            return accountUsers;
        }

        public async Task<List<string>> GetOnlineUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IdentityResult> ChangeContactInfo(ApplicationUser identityUser, VerifyUserDto verifyUser)
        {
            var userContact = await _userManager.GetPhoneNumberAsync(identityUser);

            if (identityUser.PhoneNumber == null)
            {
                var addContact = await _userManager.SetPhoneNumberAsync(identityUser, verifyUser.PhoneNumber);
                return addContact;
            }
            else if (identityUser.PhoneNumber != userContact)
            {
                var updateContact = await _userManager.SetPhoneNumberAsync(identityUser, verifyUser.PhoneNumber);
                return updateContact;
            }

            return IdentityResult.Failed();
        }
    }
}
