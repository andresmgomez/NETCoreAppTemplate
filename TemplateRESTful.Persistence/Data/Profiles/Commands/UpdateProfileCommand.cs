using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Operations.Generic;
using TemplateRESTful.Persistence.Operations.Profiles;
using TemplateRESTful.Infrastructure.Server;

namespace TemplateRESTful.Persistence.Data.Profiles.Commands
{
    public partial class UpdateProfileCommand : IRequest<ServerResponse<int>>
    {
        public int Id { get; set; }
        public string MiddleName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ServerResponse<int>>
    {
        private readonly IOnlineProfileRepository _profileRepository;
        private IUnitOfWork _unitOfWork { get; set; }
        private readonly IMapper _mapper;

        public UpdateProfileCommandHandler(
           IOnlineProfileRepository profileRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerResponse<int>> Handle(
            UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var currentProfile = await _profileRepository.GetSingleProfile(request.Id);

            if (currentProfile != null)
            {
                var onlineProfile = _mapper.Map<OnlineProfile>(request);
                await _profileRepository.UpdateOnlineProfile(onlineProfile);
                await _unitOfWork.Commit(cancellationToken);

                return ServerResponse<int>.SuccessMessage(currentProfile.Id);
            }

            return ServerResponse<int>.FailedMessage("Unable to find your Profile information");
        }
    }
}
