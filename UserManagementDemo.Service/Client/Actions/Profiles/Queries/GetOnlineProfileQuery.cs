using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Infrastructure.Server;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.Service.Client.Actions.Queries
{
    public class GetOnlineProfileQuery : IRequest<ServerResponse<OnlineProfile>>
    {
        public string Id { get; set; }
        public string EmailAddress { get; set; }

        public class GetOnlineProfileQueryHandler : IRequestHandler<GetOnlineProfileQuery, ServerResponse<OnlineProfile>>
        {
            private readonly IOnlineProfileService _profileManager;
            private readonly IMapper _mapper;

            public GetOnlineProfileQueryHandler(
                IOnlineProfileService profileManager, IMapper mapper)
            {
                _profileManager = profileManager;
                _mapper = mapper;
            }

            public async Task<ServerResponse<OnlineProfile>> Handle(
                GetOnlineProfileQuery query, CancellationToken cancellationToken)
            {
                var onlineProfile = await _profileManager.GetByIdAsync(query.Id);
                var mapOnlineProfile = _mapper.Map<OnlineProfile>(onlineProfile);

                return ServerResponse<OnlineProfile>.SuccessMessage(mapOnlineProfile);
            }
        }
    }
}
