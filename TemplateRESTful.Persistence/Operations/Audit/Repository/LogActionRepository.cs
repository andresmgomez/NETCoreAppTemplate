using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;

using TemplateRESTful.Domain.Entities.Models.Features.Audit;
using TemplateRESTful.Persistence.Operations.Generic;

namespace TemplateRESTful.Persistence.Operations.Audit
{
    public class LogActionRepository : ILogActionRepository
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositoryAsync<AuditAccount> _entityAction;

        public LogActionRepository(IMapper mapper, IGenericRepositoryAsync<AuditAccount> entityAction)
        {
            _mapper = mapper;
            _entityAction = entityAction;
        }

        public async Task<List<AuditAccount>> GetEntityLogsAsync(string userId)
        {
            var auditLogs = await _entityAction.Entities.Where(action => action.UserId == userId)
                .OrderByDescending(action => action.Id).Take(50).ToListAsync();

            var mapAuditLogs = _mapper.Map<List<AuditAccount>>(auditLogs);

            return mapAuditLogs;
        }

        public async Task AddEntityLogAsync(string action, string userId)
        {
            var auditUser = new AuditAccount()
            {
                Type = action,
                UserId = userId,
                DateTime = DateTime.UtcNow
            };

            await _entityAction.CreateAsync(auditUser);
        }
    }
}
