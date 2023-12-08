using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace TemplateRESTful.Infrastructure.Utilities
{
    public class WebBrowserUtility
    {
        public static string DecodeHyperlinkCode(string code)
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            return code;
        }

        public static string EncodeHyperlinkCode(string code)
        {
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            return code;
        }
    }
}
