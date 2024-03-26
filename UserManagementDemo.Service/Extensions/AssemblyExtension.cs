using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace UserManagementDemo.Service.Extension
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
            services.AddAutoMapper(Assembly.GetAssembly(typeof(Client.Mappings.MapIdentityProfile)));
            services.AddAutoMapper(Assembly.GetAssembly(typeof(Client.Mappings.MapProfileActions)));
        }
    }
}
