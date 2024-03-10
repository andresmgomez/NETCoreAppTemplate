using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Service.Features.Notifications
{
    public interface INotyfNotificationService
    {
        void SuccessMessage(string message, int? durationInSeconds = null);
        void ErrorMessage(string message, int? durationInSeconds = null);
        void InfoMessage(string message, int? durationInSeconds = null);
        void WarningMessage(string message, int? durationInSeconds = null);

        IEnumerable<NotyfNotification> GetNotifications();
        IEnumerable<NotyfNotification> ReadNotifications();
        void ClearNotifications();
    }
}
