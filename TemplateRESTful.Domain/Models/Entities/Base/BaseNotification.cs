using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Enums.Features;

namespace TemplateRESTful.Domain.Models
{
    public class BaseNotification
    { 
        public string Message { get; set; }
        public int? Duration { get; set;}
        public NotificationType Type { get; set; }
        public BaseNotification(NotificationType type, string message, int? durationInSeconds)
        {
            Type = type;
            Message = message;
            Duration = (durationInSeconds == null || durationInSeconds == 0 ? null : durationInSeconds * 1000);
        }

    }
}
