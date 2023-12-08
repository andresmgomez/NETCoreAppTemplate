using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Domain.Extension
{
    public static class EnumExtension
    {
        public static string ToDescriptionString<T>(this T value) where T : Enum
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] fieldAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (fieldAttributes != null && fieldAttributes.Length > 0)
            {
                return fieldAttributes[0].Description;
            }

            return value.ToString();
        }
    }
}
