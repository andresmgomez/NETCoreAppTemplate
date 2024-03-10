using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Operations.Generic;
using TemplateRESTful.Persistence.Operations.Profiles;

namespace TemplateRESTful.Persistence.Data.Profiles.Commands
{
    public partial class CreateProfileCommand : IRequest<ServerResponse<int>>
    {
        public string MiddleName { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
    }

    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ServerResponse<int>>
    {
        private readonly IOnlineProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork _unitOfWork { get; set; }

        public CreateProfileCommandHandler(
            IOnlineProfileRepository profileRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerResponse<int>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            var onlineProfile = _mapper.Map<OnlineProfile>(request);
            await _profileRepository.CreateOnlineProfile(onlineProfile);
            await _unitOfWork.Commit(cancellationToken);

            return ServerResponse<int>.SuccessMessage(onlineProfile.Id);
        }
    }
}
