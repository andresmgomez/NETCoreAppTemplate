using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UserManagementDemo.Service.Client.Features;

namespace UserManagementDemo.Web.Shared.ViewModels
{
    public class NotyfNotificationVM
    {
        public string Configuration { get; set; }
        public IEnumerable<NotyfNotification> Notifications { get; set; }
    }
}
