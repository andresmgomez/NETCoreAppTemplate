using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Domain.Models.Entities;
using TemplateRESTful.Persistence.Storage;

namespace TemplateRESTful.Web.Extensions
{
    public static class InfrastructureExtension
    {
        public static void AddInfrastructureLayer(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthenticationScheme(configuration);
            services.AddAuthorizationSettings(configuration);
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

        private static void AddAuthorizationSettings(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultUI().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/users/auth/login";
            });

            services.Configure<DataProtectionTokenProviderOptions>(options =>
               options.TokenLifespan = TimeSpan.FromHours(24));
        }
    }
}
