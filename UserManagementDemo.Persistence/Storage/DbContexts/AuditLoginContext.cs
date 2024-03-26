using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Entities.Models;
using UserManagementDemo.Domain.Models.Entities.Base;
using UserManagementDemo.Domain.Models.DTOs;

namespace UserManagementDemo.Persistence.Storage.DbContexts
{
    public abstract class AuditLoginContext : DbContext
    {
        public DbSet<AuditLoginDto> AuditUserAccess { get; set; }

        public AuditLoginContext(DbContextOptions options) : base(options) { }

        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            List<BaseAudit> auditUsers = OnBeforeSaveChanges(userId);
            int auditResult = await base.SaveChangesAsync();
            await OnAfterSaveChanges(auditUsers);

            return auditResult;
        }

        private List<BaseAudit> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            List<BaseAudit> listAudits = new List<BaseAudit>();

            foreach (EntityEntry audit in ChangeTracker.Entries())
            {
                BaseAudit baseAudit = new BaseAudit(audit);
                baseAudit.UserId = userId;

                listAudits.Add(baseAudit);

                foreach (PropertyEntry property in audit.Properties)
                {
                    if (property.IsTemporary)
                    {
                        baseAudit.PropertyEntries.Add(property);
                        continue;
                    }
                }
            }

            return listAudits.Where((BaseAudit _) => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<BaseAudit> auditUser)
        {
            if (auditUser == null || auditUser.Count == 0)
            {
                return Task.CompletedTask;
            }

            return SaveChangesAsync();
        }
    }
}
