using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Service.Server.Interfaces;

namespace UserManagementDemo.Service.Client.Actions.Commands
{
    public partial class TrackLoginCommand : IRequest<ServerResponse<int>>
    {
        public string Action { get; set; }
        public string userId { get; set; }

        public class TrackLoginCommandHandler : IRequestHandler<TrackLoginCommand, ServerResponse<int>>
        {
            private readonly ILogActionService _logActions;
            private IUnitOfWork _unitOfWork;

            public TrackLoginCommandHandler(ILogActionService logActions, IUnitOfWork unitOfWork)
            {
                _logActions = logActions;
                _unitOfWork = unitOfWork;
            }

            public async Task<ServerResponse<int>> Handle(TrackLoginCommand request, CancellationToken cancellationToken)
            {
                await _logActions.AddEntityLogAsync(request.Action, request.userId);
                await _unitOfWork.Commit(cancellationToken);

                return ServerResponse<int>.SuccessMessage(1);
            }
        }
    }
}