using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Extension;
using TemplateRESTful.Domain.Enums.Features;

namespace TemplateRESTful.Domain.Notifications.Toast
{
    public class ToastConfiguration
    {
        public ToastConfiguration(int durationInSeconds, 
            ToastGravity gravity = ToastGravity.Bottom, ToastPosition position = ToastPosition.Right)
        {
            this.duration = durationInSeconds * 1000;
            this.gravity = gravity.ToDescriptionString();
            this.position = position.ToDescriptionString();
        }
        
        #pragma warning disable IDE1006 // Naming Styles
        public string text { get; set; }
        #pragma warning restore IDE1006 // Naming Styles
        public string gravity { get; set; }
        #pragma warning disable IDE1006 // Naming Styles
        public string position { get; set; }
        #pragma warning disable IDE1006 // Naming Styles
        public string backgroundColor { get; set; }
        public int? duration { get; set; }
        public NotificationType Type { get; set; }
    }
}
