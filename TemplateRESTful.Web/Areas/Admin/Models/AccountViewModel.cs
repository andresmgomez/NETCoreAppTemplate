using System;
using System.Collections.Generic;

namespace TemplateRESTful.Web.Areas.Admin.Models
{
    public class AccountViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; } = true;
        public byte[] ProfilePicture { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
