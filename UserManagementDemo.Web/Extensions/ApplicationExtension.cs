using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using UserManagementDemo.Domain.Models.DTOs;
using UserManagementDemo.Service.Server.Features;
using UserManagementDemo.Service.Server.Interfaces;

namespace UserManagementDemo.Web.Extensions
{
    public static class ApplicationExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationContexts(configuration);
            services.AddApplicationServices(configuration);
        }

        private static void AddApplicationContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.TryAddSingleton<HttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
        }

        private static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettingsDto>(configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
