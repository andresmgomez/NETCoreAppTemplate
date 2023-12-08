using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Entities.DTOs.Account;

namespace TemplateRESTful.Domain.Models.Account
{
    public class AuthenticateUser
    {
        [BindProperty]
        public OnlineUserDto OnlineUser { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }
}
