using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Domain.Models.Entities;

namespace TemplateRESTful.Web.Areas.Accounts.Models
{
    public class UserProfileViewModel
    {
        public ApplicationUser OnlineUser { get; set; }
        public OnlineProfile OnlineProfile { get; set; }
    }
}
