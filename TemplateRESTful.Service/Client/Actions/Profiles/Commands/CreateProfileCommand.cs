using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Infrastructure.Server;
using TemplateRESTful.Persistence.Data.Interfaces;
using TemplateRESTful.Service.Client.Interfaces;

namespace TemplateRESTful.Service.Client.Actions.Commands
{
    public partial class CreateProfileCommand : IRequest<ServerResponse<int>>
    {
        public string Id { get; set; }
        public byte[] Picture { get; set; }
        public string MiddleName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DayOfBirth { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }
    }

    public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ServerResponse<int>>
    {
        private IOnlineProfileService _profileManager;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public CreateProfileCommandHandler(
            IOnlineProfileService profileManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _profileManager = profileManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerResponse<int>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
        {
            request.Id = Guid.NewGuid().ToString();
            var onlineProfile = _mapper.Map<OnlineProfile>(request);

            await _profileManager.CreateAsync(onlineProfile);
            await _unitOfWork.Commit(cancellationToken);

            return ServerResponse<int>.SuccessMessage(onlineProfile.Id);
        }
    }
}
