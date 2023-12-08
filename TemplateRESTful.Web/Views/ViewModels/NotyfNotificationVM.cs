using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Service.Features.Notifications;

namespace TemplateRESTful.Web.Shared.ViewModels
{
    public class NotyfNotificationVM
    {
        public string Configuration { get; set; }
        public IEnumerable<NotyfNotification> NotyfNotifications { get; set; }
    }
}
