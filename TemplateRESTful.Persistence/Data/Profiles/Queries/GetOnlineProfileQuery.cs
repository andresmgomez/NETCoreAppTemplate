using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Operations.Profiles;

namespace TemplateRESTful.Persistence.Data.Profiles.Queries
{
    public class GetOnlineProfileQuery : IRequest<ServerResponse<OnlineProfile>>
    {
        public int Id { get; set; }

        public class GetOnlineProfileQueryHandler : IRequestHandler<GetOnlineProfileQuery, ServerResponse<OnlineProfile>>
        {
            private readonly IOnlineProfileRepository _profileRepository;
            private readonly IMapper _mapper;

            public GetOnlineProfileQueryHandler(
                IOnlineProfileRepository profileRepository, IMapper mapper)
            {
                _profileRepository = profileRepository;
                _mapper = mapper;
            }

            public async Task<ServerResponse<OnlineProfile>> Handle(
                GetOnlineProfileQuery query, CancellationToken cancellationToken)
            {
                var onlineProfile = await _profileRepository.GetSingleProfile(query.Id);
                var mapOnlineProfile = _mapper.Map<OnlineProfile>(onlineProfile);

                return ServerResponse<OnlineProfile>.SuccessMessage(mapOnlineProfile);
            }
        }
    }
}
