using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models;

namespace TemplateRESTful.Domain.Notifications.Notyf
{
    public class NotyfPreference
    {
        public string type { get; set; }
        public string background { get; set; }
        public string className { get; set; }
        public BaseIcon icon { get; set; }

    }
}
