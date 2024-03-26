using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Domain.Notifications.Notyf
{
    public class NotyfOptions
    {
        public int DurationInSeconds { get; set; }
        public bool IsDimissible { get; set; } = false;
        public bool HasRippleEffect { get; set; } = true;
        public NotyfPosition Position { get; set; } = NotyfPosition.TopRight;
    }
}
