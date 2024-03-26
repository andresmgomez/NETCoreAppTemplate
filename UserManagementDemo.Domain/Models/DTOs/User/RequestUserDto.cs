using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UserManagementDemo.Domain.Models.DTOs
{
    public class RequestUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
