using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Persistence.Extensions
{
    public static class AssemblyExtension
    {
        public static void AddAssemblyPackages(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        public static void AddPersistenceProfiles(this IServiceCollection services) 
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(Mappings.MapProfileActions)));
        }
    }
}
