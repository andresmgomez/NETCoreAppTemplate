using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

using UserManagementDemo.Domain.Entities.Models;
using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Enums;

namespace UserManagementDemo.Domain.Models.Entities.Base
{
    public class BaseAudit
    {
        public string UserId { get; set; }
        public UserAction UserAction { get; set; }

        public EntityEntry Entry { get; }
        public List<PropertyEntry> PropertyEntries { get; } = new List<PropertyEntry>();
        public bool HasTemporaryProperties => PropertyEntries.Any();

        public BaseAudit(EntityEntry entry)
        {
            Entry = entry;
        }

        public AuditLoginDto AuditLogin()
        {
            return new AuditLoginDto
            {
                UserId = UserId,
                Action = UserAction.ToString(),
                DateTime = DateTime.UtcNow,
            };
        }
    }
}
