using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Server
{
    public interface IServerResponse
    {
        string Message { get; set; }
        bool IsSuccessful { get; set; }
    }

    public interface IServerResponse<out T> : IServerResponse
    {
        T ApiResponse { get; }
    }
}
