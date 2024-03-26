using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using UserManagementDemo.Domain.Notifications.Notyf;
using UserManagementDemo.Infrastructure.Client.Constants;
using UserManagementDemo.Infrastructure.Utilities;
using UserManagementDemo.Service.Client.Features;
using UserManagementDemo.Web.Shared.ViewModels;

namespace UserManagementDemo.Web.Middlewares
{
    internal class NotyfMiddleware : IMiddleware
    {
        public NotyfSettings _notyfSettings { get; }
        private readonly NotificationService _notyfNotification;
        private const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";

        public NotyfMiddleware(
            NotificationService notyfNotification, NotyfSettings notyfSettings)
        {
            _notyfNotification = notyfNotification;
            _notyfSettings = notyfSettings;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.OnStarting(AjaxCallback, context);
            await next(context);
        }

        private Task AjaxCallback(object context)
        {
            var httpContext = context as HttpContext;

            if (httpContext.Request.isNotyfAjaxRequest())
            {
                var notifications = new NotyfNotificationVM
                {
                    Configuration = _notyfSettings.ToJson(),
                    Notifications = _notyfNotification.ReadNotifications()
                };

                if (notifications.Notifications != null && notifications.Notifications.Any())
                {
                    var accessControlExposeHeaders = $"{GetControlExposeHeaders(httpContext.Response.Headers)}";
                    httpContext.Response.Headers.Add(AccessControlExposeHeaders, accessControlExposeHeaders);

                    var jsonNotifications = notifications.Notifications.ToJson();
                    httpContext.Response.Headers.Add(FeatureConstants.NotyfResponseHeaderKey, WebUtility.UrlEncode(jsonNotifications));
                }
            }

            return Task.FromResult(0);
        }

        private object GetControlExposeHeaders(IHeaderDictionary headers)
        {
            var existingHeaders = headers[AccessControlExposeHeaders];

            if (string.IsNullOrEmpty(existingHeaders))
            {
                return FeatureConstants.NotyfResponseHeaderKey;
            }

            return $"{existingHeaders}, {FeatureConstants.NotyfResponseHeaderKey}";
        }
    }
}
