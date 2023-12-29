using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.Persistence.Data.Actions
{
    public partial class TrackActivityActions : IRequest<ServerResponse<int>>
    {
        public string Action { get; set; }
        public string userId { get; set; }
    }

    public class TrackActivityActionsHandler : IRequestHandler<TrackActivityActions, ServerResponse<int>>
    {
        private IUnitOfWork _unitOfWork;

        public TrackActivityActionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerResponse<int>> Handle(TrackActivityActions request, CancellationToken cancel)
        {
            await _unitOfWork.Commit(cancel);
            return ServerResponse<int>.SuccessMessage(1);
        }
    }
}
