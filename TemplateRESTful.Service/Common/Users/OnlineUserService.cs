using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Service.Common.Users
{
    public class OnlineUserService : IOnlineUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public OnlineUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
