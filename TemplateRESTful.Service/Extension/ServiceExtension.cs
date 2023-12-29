using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Infrastructure.Identity;
using TemplateRESTful.Persistence.Data.Contexts;
using TemplateRESTful.Service.Common.Account;
using TemplateRESTful.Service.Common.Identity;

namespace TemplateRESTful.Service.Extension
{
    public static class ServiceExtension
    {
        public static void AddServiceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityServices(configuration);
        }

        private static void AddEntityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddTransient<IAccessUserContext, AccessUserContext>();
        }
    }
}
