using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Domain.Models.Features;

namespace TemplateRESTful.Domain.Entities.Models.Account
{
    public class UserAccount : AuditEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; } = false;
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }
    }
}
