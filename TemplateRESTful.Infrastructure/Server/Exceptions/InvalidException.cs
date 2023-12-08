using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Server
{
    public class InvalidException : Exception
    {
        public InvalidException() : base() { }
        public InvalidException(string message) : base(message) {}
        public InvalidException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args)) {}
    }
}
