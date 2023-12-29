using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Entities.Models.Features.Audit;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Data.General.IRepository;

namespace TemplateRESTful.Service.Common.Audit
{
    public class AuditUserActivity :IRequest<ServerResponse<List<AuditAccount>>>
    {
        public string userId { get; set; }
    }

    public class AuditUserActivityHandler : IRequestHandler<AuditUserActivity, ServerResponse<List<AuditAccount>>>
    {
        private readonly ILogEntityActions _logEntity;

        public AuditUserActivityHandler(ILogEntityActions logEntity)
        {
            _logEntity = logEntity;
        }

        public async Task<ServerResponse<List<AuditAccount>>> Handle(
            AuditUserActivity userActivity, CancellationToken cancelToken)
        {
            var auditusers = await _logEntity.GetEntityLogsAsync(userActivity.userId);
            return ServerResponse<List<AuditAccount>>.SuccessMessage(auditusers);
        }
    }
}
