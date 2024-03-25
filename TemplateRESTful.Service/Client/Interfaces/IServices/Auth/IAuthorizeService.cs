using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Service.Client.Interfaces
{
    public interface IAuthorizeService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterUserDto registerUser);
        Task<SignInResult> LoginUserAsync(LoginUserDto loginUser, bool allowLockout);
        Task<string> SendConfirmationCodeAsync(ApplicationUser currentAccount);
        Task<string> SendPasswordResetTokenAsync(ApplicationUser currentAccount);
        Task<IdentityResult> ConfirmUserAsync(ApplicationUser userAccount, string confirmationCode);
        Task<IdentityResult> ResetUserPasswordAsync(ApplicationUser userAccount, string resetToken, string userPassword);
    }
}
