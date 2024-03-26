using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Infrastructure.Utilities
{
    public static class WebFormatUtility
    {
        public static string FormatStringAsCode(string unformattedString)
        {
            var newString = new StringBuilder();
            int currentPosition = 0;

            while(currentPosition + 4 < unformattedString.Length) 
            {
                newString.Append(unformattedString.AsSpan(currentPosition, 4)).Append(' ');
                currentPosition += 4;
            }

            if (currentPosition < unformattedString.Length) 
            {
                newString.Append(unformattedString.AsSpan(currentPosition));
            }

            return newString.ToString().ToLowerInvariant();
        }
    }
}
