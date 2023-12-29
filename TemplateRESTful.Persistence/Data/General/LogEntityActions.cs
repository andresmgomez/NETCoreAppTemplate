using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.Models.Features.Audit;
using TemplateRESTful.Persistence.Data.General.IRepository;

namespace TemplateRESTful.Persistence.Data.General
{
    public class LogEntityActions : ILogEntityActions
    {
        private readonly IMapper _mapper;
        private readonly IEntityAsyncActions<AuditAccount> _entityAction;

        public LogEntityActions(IMapper mapper, IEntityAsyncActions<AuditAccount> entityAction)
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

            await _entityAction.AddEntityAsync(auditUser);
        }


    }
}
