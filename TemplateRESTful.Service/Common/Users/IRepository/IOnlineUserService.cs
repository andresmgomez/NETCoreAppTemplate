using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Service.Common.Users
{
    public interface IOnlineUserService
    {
        Task<IdentityResult> ChangeContactInfo(ApplicationUser identityUser, VerifyUserDto verifyUser);
    }
}
