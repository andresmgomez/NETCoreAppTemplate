using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class VerifyUserDto
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
