using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Service.Client.Interfaces
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
