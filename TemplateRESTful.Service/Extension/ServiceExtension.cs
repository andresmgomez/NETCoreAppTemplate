using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Infrastructure.Client.Services;
using TemplateRESTful.Persistence.Data.Contexts;
using TemplateRESTful.Service.Common.Email;
using TemplateRESTful.Service.Common.Identity;

namespace TemplateRESTful.Service.Extension
{
    public static class ServiceExtension
    {
        public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSecurityServices(configuration);
        }

        private static void AddSecurityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAccessUserContext, AccessUserContext>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
        }
    }
}
