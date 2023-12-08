using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TemplateRESTful.Domain.Entities.DTOs.Settings;
using TemplateRESTful.Domain.Models.Account;
using TemplateRESTful.Infrastructure.Server.Requests;
using TemplateRESTful.Infrastructure.Server.Requests.IRepository;
using TemplateRESTful.Persistence.Storage;
using TemplateRESTful.Persistence.Storage.DbContexts;
using TemplateRESTful.Service.Common.Account;

namespace TemplateRESTful.Web.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructureLayer(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCoreRepositories(configuration);
            services.AddPersistenceContexts(configuration);
            services.AddAuthenticationScheme(configuration);
        }

        private static void AddCoreRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthorizeService, AuthorizeService>();
            services.Configure<EmailSettingsDto>(configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailRequest, EmailRequest>();
        }

        private static void AddAuthenticationScheme(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc(options =>
            {
                var authPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(authPolicy));
            });
        }

        private static void AddPersistenceContexts(
            this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb"));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
               options.TokenLifespan = TimeSpan.FromHours(24));
        }
    }
}
