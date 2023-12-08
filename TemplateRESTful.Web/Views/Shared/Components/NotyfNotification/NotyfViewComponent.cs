using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Service.Features.Notifications;
using TemplateRESTful.Web.Shared.ViewModels;

namespace TemplateRESTful.Web.Shared.Components
{
    [ViewComponent(Name = "NotyfNotification")]
    public class NotyfViewComponent : ViewComponent
    {
        public NotyfSettings _notyfSettings;
        private readonly INotyfNotificationService _userNotification;

        public NotyfViewComponent(
            INotyfNotificationService userNotification, NotyfSettings notyfSettings)
        {
            _userNotification = userNotification;
            _notyfSettings = notyfSettings;
        }

        public IViewComponentResult Invoke()
        {
            var notyfNotification = new NotyfNotificationVM
            {
                Configuration = _notyfSettings.ToJson(),
                NotyfNotifications = _userNotification.ReadNotifications(),
            };

            return View("Default", notyfNotification);
        }
    }
}
