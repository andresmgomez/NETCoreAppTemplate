using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Enums.Features;
using TemplateRESTful.Domain.Models;

namespace TemplateRESTful.Service.Client.Features
{
    public class NotyfNotification : BaseNotification
    {
        public NotyfNotification(NotificationType type, string message, int? durationInSeconds) : base(type, message, durationInSeconds) { }
    }
}
