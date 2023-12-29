using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TemplateRESTful.Infrastructure.Mapping;

namespace TemplateRESTful.Web.Extensions
{
    public static class ApplicationExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationContext(configuration);
            services.AddSocialAuthentication(configuration);
            services.AddTwoFactorAuthentication(configuration);
        }

        private static void AddSocialAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication().AddGoogle("Sign in with Google", options =>
            {
                var googleAuth = configuration.GetSection("Authentication:Google");
                options.ClientId = googleAuth["ClientID"];
                options.ClientSecret = googleAuth["ClientSecret"];
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });
        }

        private static void AddTwoFactorAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
                options.AddPolicy("TwoFactorEnabled", actions => actions.RequireClaim("amr", "mfa")));    
        }

        private static void AddApplicationContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<HttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IMappingViewer, MappingViewer>();
        }
    }
}
