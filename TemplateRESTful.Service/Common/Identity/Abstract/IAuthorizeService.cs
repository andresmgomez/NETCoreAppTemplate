using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Entities.DTOs.Account;
using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Domain.Models.Users;
using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.Service.Common.Account
{
    public interface IAuthorizeService
    {
        //Task<ServerResponse<OnlineAccountDto>> GetTokenAsync(LoginUser loginUser, string ipAddress);
        Task<IdentityResult> RegisterAccountAsync(RegisterUser registerUser);
        Task<SignInResult> LoginAccountAsync(LoginUser loginUser, bool allowLockout);
        Task<string> SendConfirmationCodeAsync(ApplicationUser currentAccount);
        Task<string> SendPasswordResetTokenAsync(ApplicationUser currentAccount);
        Task<IdentityResult> ConfirmAccountAsync(ApplicationUser userAccount, string confirmationCode);
        Task<IdentityResult> ResetPasswordAccountAsync(ApplicationUser userAccount, string resetToken, string userPassword);
    }
}
