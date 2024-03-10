using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Operations.Generic;

namespace TemplateRESTful.Persistence.Data.Actions
{
    public partial class UserActionCommand : IRequest<ServerResponse<int>>
    {
        public string Action { get; set; }
        public string userId { get; set; }

        public class UserActionCommandHandler : IRequestHandler<UserActionCommand, ServerResponse<int>>
        {
            private IUnitOfWork _unitOfWork;

            public UserActionCommandHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ServerResponse<int>> Handle(
                UserActionCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Commit(cancellationToken);
                return ServerResponse<int>.SuccessMessage(1);
            }
        }
    }
}