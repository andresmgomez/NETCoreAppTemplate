using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Domain.Enums.Features;
using UserManagementDemo.Domain.Models;

namespace UserManagementDemo.Service.Client.Features
{
    public class NotyfNotification : BaseNotification
    {
        public NotyfNotification(NotificationType type, string message, int? durationInSeconds) : 
            base(type, message, durationInSeconds) {}
    }
}
