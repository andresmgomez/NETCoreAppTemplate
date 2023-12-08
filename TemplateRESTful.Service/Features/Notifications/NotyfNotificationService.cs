using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Enums.Features;
using TemplateRESTful.Persistence.Data.Containers;

namespace TemplateRESTful.Service.Features.Notifications
{
    public class NotyfNotificationService : INotyfNotificationService
    {
        protected IUserActions<NotyfNotification> NotificationContainer;

        public NotyfNotificationService(INotificationContainer notification)
        {
            NotificationContainer = notification.CreateNotification<NotyfNotification>(); ;
        }

        public void SuccessMessage(string message, int? durationInSeconds = null) 
        {
            var successMessage = new NotyfNotification(NotificationType.Success, message, durationInSeconds);
            NotificationContainer.Add(successMessage);
        }

        public void ErrorMessage(string message, int? durationInSeconds = null)
        {
            var errorMessage = new NotyfNotification(NotificationType.Error, message, durationInSeconds);
            NotificationContainer.Add(errorMessage);
        }

        public void InfoMessage(string message, int? durationInSeconds = null)
        {
            var infoMessage = new NotyfNotification(NotificationType.Information, message, durationInSeconds);
            NotificationContainer.Add(infoMessage);
        }

        public void WarningMessage(string message, int? durationInSeconds = null)
        {
            var warningMessage = new NotyfNotification(NotificationType.Warning, message, durationInSeconds);
            NotificationContainer.Add(warningMessage);
        }

        public IEnumerable<NotyfNotification> GetNotifications()
        {
            return NotificationContainer.Get();
        }

        public IEnumerable<NotyfNotification> ReadNotifications()
        {
            return NotificationContainer.Read();
        }

        public void ClearNotifications()
        {
            NotificationContainer.Clear();
        }
    }
}
