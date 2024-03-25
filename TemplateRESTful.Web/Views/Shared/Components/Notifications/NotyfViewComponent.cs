using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using TemplateRESTful.Domain.Notifications.Notyf;
using TemplateRESTful.Infrastructure.Utilities;
using TemplateRESTful.Service.Client.Features;
using TemplateRESTful.Web.Shared.ViewModels;

namespace TemplateRESTful.Web.Shared.Components
{
    [ViewComponent(Name = "Notifications")]
    public class NotyfViewComponent : ViewComponent
    {
        public NotyfSettings _notyfSettings;
        private readonly NotificationService _userNotification;

        public NotyfViewComponent(
            NotificationService userNotification, NotyfSettings notyfSettings)
        {
            _userNotification = userNotification;
            _notyfSettings = notyfSettings;
        }

        public IViewComponentResult Invoke()
        {
            var notyfNotification = new NotyfNotificationVM
            {
                Configuration = _notyfSettings.ToJson(),
                Notifications = _userNotification.ReadNotifications(),
            };

            return View("Default", notyfNotification);
        }
    }
}
