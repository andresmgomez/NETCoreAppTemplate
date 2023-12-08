using System.Text;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

using TemplateRESTful.Infrastructure.Identity;
using TemplateRESTful.Persistence.Storage;
using TemplateRESTful.Service.Common.Account;
using TemplateRESTful.Service.Common.Identity;
using TemplateRESTful.Domain.Models.Account;

namespace TemplateRESTful.API.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityInfrastructure(configuration);
        }

        private static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));
            } else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                    migration => migration.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
                ));

            }

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

            #region Identity Service
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            #endregion
        }
    }
}
