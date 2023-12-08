using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Entities.Models.Account;

namespace TemplateRESTful.Persistence.Data.Users
{
    public interface IUserAccountRepository
    {
        IQueryable<UserAccount> UserAccounts { get; }
        Task<List<UserAccountDto>> GetUserAccounts();
    }
}
