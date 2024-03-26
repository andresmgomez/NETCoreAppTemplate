using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;
using UserManagementDemo.Service.Client.Actions.Commands;
using UserManagementDemo.Service.Client.Actions.Queries;

namespace UserManagementDemo.Service.Client.Mappings
{
    public class MapProfileActions : Profile
    {
        public MapProfileActions()
        {
            CreateMap<OnlineProfileDto, OnlineProfile>().ReverseMap();
            CreateMap<CreateProfileCommand, OnlineProfile>().ReverseMap();
            CreateMap<UpdateProfileCommand, OnlineProfile>().ReverseMap();

            CreateMap<GetOnlineProfileQuery, OnlineProfile>().ReverseMap();
        }
    }
}
