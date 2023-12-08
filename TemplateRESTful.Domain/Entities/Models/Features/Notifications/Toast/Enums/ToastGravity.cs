using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Notifications.Toast
{
    public enum ToastGravity
    {
        [Description("top")]
        Top,
        [Description("bottom")]
        Bottom
    }
}
