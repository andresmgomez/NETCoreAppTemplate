using Microsoft.AspNetCore.Mvc;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Domain.Models.Entities;

namespace UserManagementDemo.Web.Areas.Accounts.Models
{
    public class UserProfileViewModel
    {
        public ApplicationUser OnlineUser { get; set; }
        public OnlineProfile OnlineProfile { get; set; }
    }
}
