using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UserManagementDemo.Persistence.Data.Contexts;
using UserManagementDemo.Service.Client.Entities;
using UserManagementDemo.Service.Client.Interfaces;
using UserManagementDemo.Service.Server.Features;
using UserManagementDemo.Service.Server.Interfaces;

namespace UserManagementDemo.Service.Extension
{
    public static class ServiceExtension
    {
        public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSecurityServices(configuration);
            services.AddUserServices(configuration);
        }

        private static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();

            services.AddTransient<IAccessUserContext, AccessUserContext>();
            services.AddTransient<ILogActionService, LogActionService>();
        }

        private static void AddUserServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOnlineUserService, OnlineUserService>();
            services.AddScoped<IOnlineProfileService, OnlineProfileService>();
        }
    }
}
