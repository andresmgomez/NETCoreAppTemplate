﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TemplateRESTful.Persistence.Data.Contexts
{
    public class AccessUserContext : IAccessUserContext
    {
        public AccessUserContext(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier) != null)
            {
                UserId = contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            if (contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name) != null)
            {
                UserName = contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name).Value;
            }
        }
        public string UserId { get; }

        public string UserName { get; }

    }
}
