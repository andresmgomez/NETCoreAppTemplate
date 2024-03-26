using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Service.Client.Mappings
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
