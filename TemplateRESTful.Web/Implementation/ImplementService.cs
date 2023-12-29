using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using TemplateRESTful.Service.Features.Notifications;

namespace TemplateRESTful.Web.Implementation
{
    public class ImplementService<T> : PageModel where T : class
    {
        private ILogger<T> _logger;
        private INotyfNotificationService notificationService;

        protected ILogger<T> Logger => _logger ??=
            HttpContext.RequestServices.GetService<ILogger<T>>();

        protected INotyfNotificationService _notificationService => 
            notificationService ??= HttpContext.RequestServices.
            GetService<INotyfNotificationService>();
    }
}
