using AutoMapper;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Web.Areas.Admin.Models;

namespace TemplateRESTful.Web.Areas.Admin.Mappings
{
    public class UserAccount : Profile
    {
        public UserAccount() 
        {
            CreateMap<ApplicationUser, AccountViewModel>().ReverseMap();
        }
    }
}
