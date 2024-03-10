using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Models.DTOs.User
{
    public class RecoveryUserDto
    {
        public string[] RecoveryCodes { get; set; }
        public int? RecoveryCodesLeft { get; set; }
    }
}
