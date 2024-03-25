using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Service.Client.Features;

namespace TemplateRESTful.Web.Shared.ViewModels
{
    public class NotyfNotificationVM
    {
        public string Configuration { get; set; }
        public IEnumerable<NotyfNotification> Notifications { get; set; }
    }
}
