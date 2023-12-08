using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Data.Users.IRepository;

namespace TemplateRESTful.Service.Common.Logs.Audit
{
    public class LogAuditService : IRequest<ServerResponse<List<AuditAccountDto>>>
    {
        public string userId { get; set; }
    }

    public class AuditQueryService : IRequestHandler<LogAuditService, ServerResponse<List<AuditAccountDto>>>
    {
        private readonly IAuditAccountRepository _auditRepository;

        public AuditQueryService(IAuditAccountRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task<ServerResponse<List<AuditAccountDto>>> Handle(LogAuditService request, CancellationToken cancellationToken)
        {
            var auditLogs = await _auditRepository.GetAuditAccountLogs(request.userId);
            return ServerResponse<List<AuditAccountDto>>.SuccessMessage(auditLogs);
        }
    }
}
