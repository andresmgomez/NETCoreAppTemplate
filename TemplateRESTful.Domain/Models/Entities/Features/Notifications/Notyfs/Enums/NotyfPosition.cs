using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Notifications.Notyf
{
    public enum NotyfPosition
    {
        [Description("right-top")]
        TopRight,
        [Description("right-bottom")]
        BottomRight,
        [Description("left-bottom")]
        BottomLeft,
        [Description("left-top")]
        TopLeft,
        [Description("center-top")]
        TopCenter,
        [Description("center-bottom")]
        BottomCenter
    }
}