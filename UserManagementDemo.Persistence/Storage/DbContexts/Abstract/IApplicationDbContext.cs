using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Persistence.Storage.DbContexts
{
    public interface IApplicationDbContext
    {
        IDbConnection OpenConnection { get; }
        bool HasBeenModified { get; }
        DbSet<OnlineProfile> OnlineProfiles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
