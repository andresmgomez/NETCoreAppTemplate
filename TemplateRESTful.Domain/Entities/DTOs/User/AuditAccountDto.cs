using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Entities.DTOs.User
{
    public class AuditAccountDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
    }
}
