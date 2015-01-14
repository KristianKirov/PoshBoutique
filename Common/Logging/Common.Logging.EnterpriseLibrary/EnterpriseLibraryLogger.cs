using Common.Logging.Model;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging.EnterpriseLibrary
{
    public class EnterpriseLibraryLogger : ILogger
    {
        public static void Initialize()
        {
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());
        }

        private readonly string loggingCategory;

        public EnterpriseLibraryLogger(string category)
        {
            this.loggingCategory = category;
        }

        private void LogWrite(string message, TraceEventType severity)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Message = message;
            logEntry.Severity = severity;
            logEntry.Categories.Add(this.loggingCategory);

            Logger.Write(logEntry);
        }

        public void LogInfo(string message)
        {
            this.LogWrite(message, TraceEventType.Information);
        }

        public void LogWarning(string message)
        {
            this.LogWrite(message, TraceEventType.Warning);
        }

        public void LogError(string message)
        {
            this.LogWrite(message, TraceEventType.Error);
        }
    }
}
