using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Persistence.Data.Contexts;
using TemplateRESTful.Service.Client.Entities;
using TemplateRESTful.Service.Client.Interfaces;
using TemplateRESTful.Service.Server.Features;
using TemplateRESTful.Service.Server.Interfaces;

namespace TemplateRESTful.Service.Extension
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
