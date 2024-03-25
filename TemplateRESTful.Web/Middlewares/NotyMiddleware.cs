using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Infrastructure.Client.Constants;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Service.Client.Features;
using TemplateRESTful.Web.Shared.ViewModels;

namespace TemplateRESTful.Web.Middlewares
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
