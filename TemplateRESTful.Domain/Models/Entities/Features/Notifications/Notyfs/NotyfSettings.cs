using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TemplateRESTful.Domain.Models;

namespace TemplateRESTful.Domain.Notifications.Notyf
{
    public class NotyfSettings
    {
        //
        public int duration { get; set; }
        public bool dismissible { get; set; }
        public bool ripple { get; set; }
        public List<NotyfPreference> types { get; set; }

        //
        public NotyfSettings(int durationInSeconds = 5, 
            NotyfPosition alertPosition = NotyfPosition.BottomRight, bool isDismissible = true)
        {
            duration = (durationInSeconds > 0) ? durationInSeconds * 1000 : 5000;
            dismissible = isDismissible;
            ripple = true;

            types = new List<NotyfPreference>()
            {
                new NotyfPreference
                {
                    type = "success",
                    background = "#28a745"
                },
                new NotyfPreference
                {
                    type = "error",
                    background = "#dc3545"
                },
                new NotyfPreference
                {
                    type = "warning",
                    background = "#eeb718",
                    icon = new BaseIcon
                    {
                         className = "fa fa-warning text-white",
                         tagName = "i",
                    }
                },
                new NotyfPreference
                {
                    type = "info",
                    background = "#231be9",
                    icon = new BaseIcon
                    {
                         className = "fa fa-info text-white",
                         tagName = "i",
                    }
                },
            };


            try
            {
                string description = ToDescriptionString(alertPosition);
                var positionArray = description.Split('-');
                
                new BasePosition()
                {
                    x = (positionArray is null) ? "right" : positionArray[0],
                    y = (positionArray is null) ? "bottom" : positionArray[1]
                };
            }
            catch
            {
                new BasePosition()
                {
                    x = "right",
                    y = "bottom"
                };
            }
        }

        private static string ToDescriptionString(NotyfPosition value)
        {
            var fieldAttributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return fieldAttributes.Length > 0 ? fieldAttributes[0].Description : "bottom-right";
        }
    }
}
