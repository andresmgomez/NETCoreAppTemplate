using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.DTOs.User;

namespace TemplateRESTful.Persistence.Data.Users.IRepository
{
    public interface IAuditAccountRepository
    {
        Task<List<AuditAccountDto>> GetAuditAccountLogs(string userId);
        Task AddAuditLog(string action, string userId);
    }
}
