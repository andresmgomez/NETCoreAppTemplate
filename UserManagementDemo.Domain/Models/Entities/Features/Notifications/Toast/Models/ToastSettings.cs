using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Notifications.Toast
{
    public class ToastSettings
    {
        public int DurationInSeconds { get; set; }
        public ToastGravity ToastGravity { get; set; }
        public ToastPosition ToastPosition { get; set; }
    }
}
