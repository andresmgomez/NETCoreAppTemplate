using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Persistence.Data.Contexts;

namespace TemplateRESTful.Persistence.Storage.DbContexts
{
    public class ApplicationDbContext : AuditLoginContext, IApplicationDbContext
    {
        private readonly IAccessUserContext _userContext;
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options, IAccessUserContext userContext) : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _userContext = userContext;
        }

        public IDbConnection OpenConnection => Database.GetDbConnection();
        public bool HasBeenModified => ChangeTracker.HasChanges();
        public DbSet<OnlineProfile> OnlineProfiles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(_userContext.UserId);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
