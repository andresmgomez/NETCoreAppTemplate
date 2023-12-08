using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Entities.Models.Features.Audit;
using TemplateRESTful.Persistence.Data.Users.IRepository;

namespace TemplateRESTful.Persistence.Data.Users
{
    public class AuditAccountRepository : IAuditAccountRepository
    {
        private readonly IMapper _mapper;
        private readonly IEntityAsyncActions<AuditAccount> _auditAction;

        public AuditAccountRepository(IMapper mapper, IEntityAsyncActions<AuditAccount> auditAction)
        {
            _mapper = mapper;
            _auditAction = auditAction;
        }

        public async Task<List<AuditAccountDto>> GetAuditAccountLogs(string userId)
        {
            var accountLogs = await _auditAction.Entities.Where(account => account.UserId == userId)
                .OrderByDescending(account => account.Id).Take(250).ToListAsync();

            var mapAccountLogs = _mapper.Map<List<AuditAccountDto>>(accountLogs);
            return mapAccountLogs;
        }

        public async Task AddAuditLog(string action, string userId)
        {
            var auditAccount = new AuditAccount()
            {
                UserId = userId,
                Action = action,
                DateTime = DateTime.UtcNow
            };

            await _auditAction.AddEntityAsync(auditAccount);
        }
    }
}
