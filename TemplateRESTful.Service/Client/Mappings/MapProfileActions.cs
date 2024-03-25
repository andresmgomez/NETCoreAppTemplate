using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Service.Client.Actions.Commands;
using TemplateRESTful.Service.Client.Actions.Queries;

namespace TemplateRESTful.Service.Client.Mappings
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
