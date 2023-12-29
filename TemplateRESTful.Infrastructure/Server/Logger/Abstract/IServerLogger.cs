using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateRESTful.Infrastructure.Server
{
    public interface IServerLogger
    {
        void InformationLog(string message);
        void WarningLog(string message);
        void DebuggingLog(string message);
        void CriticalLog(string message);
    }
}
