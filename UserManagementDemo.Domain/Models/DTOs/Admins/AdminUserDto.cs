using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class AdminUserDto
    {
        public string EmailAccount { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator Code")]
        public string AccessCode { get; set; }
        public bool KeepSession { get; set; } = true;
        public bool RememberBrowser { get; set; } = false;
    }
}
