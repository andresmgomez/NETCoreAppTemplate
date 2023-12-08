using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Entities.Models.Account;

namespace TemplateRESTful.Persistence.Data.Users
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly IEntityAsyncActions<UserAccount> _userAccount;

        public UserAccountRepository(IEntityAsyncActions<UserAccount> userAccount)
        {
            _userAccount = userAccount;
        }

        public IQueryable<UserAccount> UserAccounts => _userAccount.Entities;

        public async Task<List<UserAccountDto>> GetUserAccounts()
        {
            var userAccounts = await _userAccount.Entities.Select(
                userAccount => new UserAccountDto
                {
                    Id = userAccount.Id,
                    FirstName = userAccount.FirstName,
                    LastName = userAccount.LastName,
                    IsVerified = userAccount.IsVerified,
                }).ToListAsync();

            return userAccounts;
        }
    }
}