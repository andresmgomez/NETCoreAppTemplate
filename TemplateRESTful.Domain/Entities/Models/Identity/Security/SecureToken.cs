using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Models.Account
{
    public class SecureToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool hasExpired => DateTime.UtcNow >= Expires;
        public DateTime CreatedOn { get; set; }
        public string CreatedIP { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedIP { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !hasExpired;
    }
}
