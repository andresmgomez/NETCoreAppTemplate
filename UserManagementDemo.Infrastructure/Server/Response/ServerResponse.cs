using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementDemo.Infrastructure.Server
{
    public class ServerResponse : IServerResponse
    {
        public string Message { get; set; }
        public bool IsSuccessful { get; set; }

        public static IServerResponse FailedMessage()
        {
            return new ServerResponse
            {
                IsSuccessful = false
            };
        }

        public static IServerResponse FailedMessage(string message)
        {
            return new ServerResponse
            {
                IsSuccessful = false,
                Message = message
            };
        }

        public static IServerResponse SuccessMessage()
        {
            return new ServerResponse
            {
                IsSuccessful = true
            };
        }

        public static IServerResponse SuccessMessage(string message)
        {
            return new ServerResponse
            {
                IsSuccessful = true,
                Message = message
            };
        }
    }
}
