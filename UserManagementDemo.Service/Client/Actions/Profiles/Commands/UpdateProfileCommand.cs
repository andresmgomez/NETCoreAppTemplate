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
using UserManagementDemo.Persistence.Data.Interfaces;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.Service.Client.Actions.Commands
{
    public partial class UpdateProfileCommand : IRequest<ServerResponse<int>>
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

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ServerResponse<int>>
    {
        private IOnlineProfileService _profileManager;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }

        public UpdateProfileCommandHandler(
           IOnlineProfileService profileManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _profileManager = profileManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServerResponse<int>> Handle(
            UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var currentProfile = await _profileManager.GetByIdAsync(request.Id);

            if (currentProfile != null)
            {
                var onlineProfile = _mapper.Map<OnlineProfile>(request);
                await _profileManager.UpdateAsync(onlineProfile);
                await _unitOfWork.Commit(cancellationToken);

                return ServerResponse<int>.SuccessMessage(currentProfile.Id);
            }

            return ServerResponse<int>.FailedMessage("Unable to find your Profile information");
        }
    }
}
