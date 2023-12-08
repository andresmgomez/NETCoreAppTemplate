using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Server
{
    public interface IErrorException
    {
        void NullArgument<T>(T value, string propertyName);
        void NullArgument<T>(T value, string propertyName, string message);
        void NotNullArgument<T>(T value, string message);
        void FalseArgument(bool value, string message);
    }
}
