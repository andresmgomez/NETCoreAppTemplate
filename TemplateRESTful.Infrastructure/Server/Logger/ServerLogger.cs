using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using ILogger = NLog.ILogger;

namespace TemplateRESTful.Infrastructure.Server
{
    public class ServerLogger :IServerLogger
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void InformationLog(string message) => logger.Info(message);
        public void WarningLog(string message) => logger.Warn(message);
        public void DebuggingLog(string message) => logger.Debug(message);
        public void CriticalLog(string message) => logger.Error(message);
    }
}
