using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Infrastructure.Data.Actions;
using TemplateRESTful.Infrastructure.Data.Containers;
using TemplateRESTful.Service.Client.Features;
using TemplateRESTful.Web.Middlewares;

namespace TemplateRESTful.Web.Extensions
{
    public static class NotificationsExtension
    {
        public static void AddNotyfNotification(this IServiceCollection services, Action<NotyfOptions> configure)
        {
            var notifyOptions = new NotyfOptions();
            configure(notifyOptions);

            var notyfSettings = new NotyfSettings(notifyOptions.DurationInSeconds, notifyOptions.Position, notifyOptions.IsDimissible);

            if (notyfSettings == null)
            {
                throw new ArgumentNullException(nameof(notyfSettings));
            }

            services.AddFrameworkServices();

            services.AddSingleton<ITempActionsContainer, TemporaryActions>();
            
            services.AddSingleton<INotificationContainer, NotificationContainer>();
            services.AddScoped<NotificationService, NotificationService>();
            services.AddScoped<NotyfMiddleware>();
            
            services.AddSingleton(notyfSettings);
        }

        private static void AddFrameworkServices(this IServiceCollection services)
        {
            #region Framework Services
            //Check if a TempDataProvider is already registered.
            var tempDataProvider = services.FirstOrDefault(d => d.ServiceType == typeof(ITempDataProvider));
            if (tempDataProvider == null)
            {
                //Add a tempdata provider when one is not already registered
                services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            }
            //check if IHttpContextAccessor implementation is not registered. Add one if not.
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }
            #endregion
        }
    }
}
