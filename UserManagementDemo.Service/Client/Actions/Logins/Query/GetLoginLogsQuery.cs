﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Service.Server.Interfaces;

namespace UserManagementDemo.Service.Client.Actions.Queries
{
    public partial class GetLoginLogsQuery : IRequest<ServerResponse<List<AuditLoginDto>>>
    {
        public string userId { get; set; }
    }

    public class GetLoginLogsQueryHandler : IRequestHandler<GetLoginLogsQuery, ServerResponse<List<AuditLoginDto>>>
    {
        private readonly ILogActionService _userAction;

        public GetLoginLogsQueryHandler(ILogActionService userAction)
        {
            _userAction = userAction;
        }

        public async Task<ServerResponse<List<AuditLoginDto>>> Handle(GetLoginLogsQuery request, CancellationToken cancellationToken)
        {
            var auditActions = await _userAction.GetEntityLogsAsync(request.userId);
            return ServerResponse<List<AuditLoginDto>>.SuccessMessage(auditActions);
        }
    }
}
