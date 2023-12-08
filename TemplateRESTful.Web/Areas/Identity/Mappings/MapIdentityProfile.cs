using AutoMapper;
using TemplateRESTful.Domain.Entities.DTOs.User;
using TemplateRESTful.Domain.Entities.Models.Features.Audit;

namespace TemplateRESTful.Web.Areas.Identity.Mappings
{
    public class MapIdentityProfile : Profile
    {
        public MapIdentityProfile()
        {
            CreateMap<AuditAccountDto, AuditAccount>().ReverseMap();
        }
    }
}
