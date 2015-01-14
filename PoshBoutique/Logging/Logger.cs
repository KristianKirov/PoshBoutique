using Common.Logging.EnterpriseLibrary;
using Common.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Logging
{
    public static class Logger
    {
        private const string LOGGING_CATEGORY = "PoshBoutique";

        public static ILogger Current { get; private set; }

        static Logger()
        {
            EnterpriseLibraryLogger.Initialize();
            Logger.Current = new EnterpriseLibraryLogger(Logger.LOGGING_CATEGORY);
        }
    }
}