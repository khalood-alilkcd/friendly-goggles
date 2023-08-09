using Contracts;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerServices
{
    public class LoggerManager : ILoggerManager
    {
        ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {
            
        }
        public void LogDebug(string message) => logger.Debug(message);

        public void LogError(string message) => logger.Error(message);

        public void LogInfo(string message) => logger.Info(message);

        public void LogWorn(string message) => logger.Warn(message);
    }
}
