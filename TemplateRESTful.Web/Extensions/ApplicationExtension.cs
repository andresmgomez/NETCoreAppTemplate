using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using TemplateRESTful.Domain.Models.DTOs;
using TemplateRESTful.Infrastructure.Client.Services;
using TemplateRESTful.Service.Common.Email;

namespace TemplateRESTful.Web.Extensions
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
            services.AddScoped<IRenderRazorView, RenderRazorView>();
            services.Configure<EmailSettingsDto>(configuration.GetSection("EmailConfiguration"));
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
