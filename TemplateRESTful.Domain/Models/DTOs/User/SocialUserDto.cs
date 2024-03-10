using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Models.DTOs
{
    public class SocialUserDto
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public ClaimsPrincipal ClaimPrincipal { get; set; }
    }
}
