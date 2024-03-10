using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Domain.Mappings
{
    public class MapIdentityProfile : Profile
    {
        public MapIdentityProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>()
               .ForMember(user => user.UserName, option => option.MapFrom(field => field.Email));
        }
    }
}
