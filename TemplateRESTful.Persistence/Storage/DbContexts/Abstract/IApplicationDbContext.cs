using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Entities.Models.Account;

namespace TemplateRESTful.Persistence.Storage.DbContexts
{
    public interface IApplicationDbContext
    {
        IDbConnection Connection { get; }
        bool HasChanges { get; }

        DbSet<UserAccount> UserAccounts { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
