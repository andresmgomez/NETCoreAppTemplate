using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities.Profiles;
using TemplateRESTful.Persistence.Data.Profiles.Commands;
using TemplateRESTful.Persistence.Data.Profiles.Queries;

namespace TemplateRESTful.Persistence.Mappings
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
