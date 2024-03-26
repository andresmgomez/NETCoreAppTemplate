using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Service.Server.Interfaces;

namespace UserManagementDemo.Service.Server.Features
{
    public class LogActionService : ILogActionService
    {
        private readonly IGenericRepositoryAsync<AuditLoginDto> _entityAction;
        private readonly IMapper _mapper;

        public LogActionService(IGenericRepositoryAsync<AuditLoginDto> entityAction, IMapper mapper)
        {
            _entityAction = entityAction;
            _mapper = mapper;
        }

        public async Task AddEntityLogAsync(string action, string userId)
        {
            var auditUserLogin = new AuditLoginDto()
            {
                Action = action,
                UserId = userId,
                DateTime = DateTime.UtcNow
            };

            await _entityAction.CreateAsync(auditUserLogin);
        }

        public async Task<List<AuditLoginDto>> GetEntityLogsAsync(string userId)
        {
            var auditLogs = await _entityAction.Entities.OrderByDescending(a => a.DateTime).ToListAsync();
            var mapAuditLogs = _mapper.Map<List<AuditLoginDto>>(auditLogs);

            return mapAuditLogs;
        }
    }
}
