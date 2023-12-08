using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Infrastructure.Constants;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Service.Features.Notifications;
using TemplateRESTful.Web.Shared.ViewModels;

namespace TemplateRESTful.Web.Middlewares
{
    internal class NotyfMiddleware : IMiddleware
    {
        public NotyfSettings _notyfSettings { get; }
        private readonly INotyfNotificationService _notyfNotification;
        private const string AccessControlExposeHeaders = "Access-Control-Expose-Headers";

        public NotyfMiddleware(
            INotyfNotificationService notyfNotification, NotyfSettings notyfSettings)
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
                    NotyfNotifications = _notyfNotification.ReadNotifications()
                };

                if (notifications.NotyfNotifications != null && notifications.NotyfNotifications.Any())
                {
                    var accessControlExposeHeaders = $"{GetControlExposeHeaders(httpContext.Response.Headers)}";
                    httpContext.Response.Headers.Add(AccessControlExposeHeaders, accessControlExposeHeaders);

                    var jsonNotifications = notifications.NotyfNotifications.ToJson();
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
