﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Models.DTOs
{
    public class ConfirmUserDto
    {
        public string EmailConfirmationUrl { get; set; } = null;
    }
}
