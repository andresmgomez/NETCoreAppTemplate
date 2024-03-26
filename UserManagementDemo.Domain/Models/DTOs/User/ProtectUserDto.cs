using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class ProtectUserDto
    {
        [StringLength(33, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 32)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator Code")]
        public string Token { get; set; }

        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
        public string QRCode { get; set; }
    }
}
