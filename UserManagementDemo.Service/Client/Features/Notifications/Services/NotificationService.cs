using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Enums.Features;
using UserManagementDemo.Infrastructure.Data.Containers;
using UserManagementDemo.Service.Client.Interfaces;

namespace UserManagementDemo.Service.Client.Features
{
    public class NotificationService : INotificationService
    {
        protected IUserActionsContainer<NotyfNotification> NotificationContainer;

        public NotificationService(INotificationContainer notification)
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
