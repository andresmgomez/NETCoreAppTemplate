using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Extension
{
    public static class DomainExtension
    {
        public static void AddDomainProfiles(this IServiceCollection services) 
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(
                Mappings.MapIdentityProfile)));
        }
    }
}
