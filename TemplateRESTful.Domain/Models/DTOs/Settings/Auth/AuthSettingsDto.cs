using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TemplateRESTful.Domain.Models.DTOs
{
    public class AuthSettingsDto
    {
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator Code")]
        public string TwoFactorAuthentication { get; set; }
        public bool KeepSession { get; set; } = true;
        public bool RememberBrowser { get; set; } = false;
    }
}
