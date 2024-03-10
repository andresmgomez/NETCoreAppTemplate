using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.Models.Features.Audit;

namespace TemplateRESTful.Persistence.Operations.Audit
{
    public interface ILogActionRepository
    {
        Task<List<AuditAccount>> GetEntityLogsAsync(string userId);
        Task AddEntityLogAsync(string action, string userId);
    }
}
