using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace UserManagementDemo.Infrastructure.Server
{
    public class ServerException : IErrorException
    {
        public static IErrorException throwError { get; }

         public void NullArgument<T>(T value, string propertyName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(propertyName);
            }
        }

        public void NullArgument<T>(T value, string propertyName, string message)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"{propertyName} cannot be null. {message}");
            }
        }

        public void NotNullArgument<T>(T value, string message)
        {
            if (value != null)
            {
                throw new ArgumentException(message);
            }
        }

        public void FalseArgument(bool value, string message)
        {
            if (value == false)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
