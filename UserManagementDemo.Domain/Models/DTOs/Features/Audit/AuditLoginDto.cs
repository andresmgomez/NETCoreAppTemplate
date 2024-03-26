using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class AuditLoginDto
    {
        public AuditLoginDto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public DateTime DateTime { get; set; }
    }
}
