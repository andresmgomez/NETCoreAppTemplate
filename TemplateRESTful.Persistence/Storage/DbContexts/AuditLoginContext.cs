using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Entities.Models;
using TemplateRESTful.Domain.Models.Entities.Base;
using TemplateRESTful.Domain.Models.DTOs;

namespace TemplateRESTful.Persistence.Storage.DbContexts
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
                //if (audit.Entity is AuditUser || audit.State == EntityState.Detached || audit.State == EntityState.Unchanged)
                //{
                //    continue;
                //}

                BaseAudit baseAudit = new BaseAudit(audit);
                //baseAudit.TableName = audit.Entity.GetType().Name;
                baseAudit.UserId = userId;

                listAudits.Add(baseAudit);

                foreach (PropertyEntry property in audit.Properties)
                {
                    if (property.IsTemporary)
                    {
                        baseAudit.PropertyEntries.Add(property);
                        continue;
                    }

                    //string propertyName = property.Metadata.Name;

                    //if (property.Metadata.IsPrimaryKey())
                    //{
                    //    baseAudit.KeyValues[propertyName] = property.CurrentValue;
                    //    continue;
                    //}

                    //switch (audit.State)
                    //{
                    //    case EntityState.Added:
                    //        baseAudit.UserAction = UserAction.Create;
                    //        baseAudit.NewValues[propertyName] = property.CurrentValue;
                    //        break;

                    //    case EntityState.Modified:
                    //        if (property.IsModified)
                    //        {
                    //            baseAudit.ChangedColumns.Add(propertyName);
                    //            baseAudit.UserAction = UserAction.Update;
                    //            baseAudit.OldValues[propertyName] = property.OriginalValue;
                    //            baseAudit.NewValues[propertyName] = property.CurrentValue;
                    //        }

                    //        break;
                    //}
                }
            }

            //foreach (BaseAudit auditEntry in listAudits.Where((BaseAudit _) => !_.HasTemporaryProperties))
            //{
            //    AuditUserAccess.Add(auditEntry.AuditUser());
            //}

            return listAudits.Where((BaseAudit _) => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<BaseAudit> auditUser)
        {
            if (auditUser == null || auditUser.Count == 0)
            {
                return Task.CompletedTask;
            }

            //foreach (BaseAudit auditEntry in auditUser)
            //{
            //    foreach (PropertyEntry property in auditEntry.PropertyEntries)
            //    {
            //        if (property.Metadata.IsPrimaryKey())
            //        {
            //            auditEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
            //        }

            //        auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
            //    }

            //    AuditUserAccess.Add(auditEntry.AuditUser());
            //}

            return SaveChangesAsync();
        }
    }
}
