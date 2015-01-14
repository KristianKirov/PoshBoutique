using Common.Logging.Model;
using Common.Logging.Mvc.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Common.Logging.Mvc
{
    public class FormattedExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;
        private readonly IExceptionLoggerContextFormatter formatter;

        public FormattedExceptionLogger(ILogger logger, IExceptionLoggerContextFormatter formatter)
        {
            this.logger = logger;
            this.formatter = formatter;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            ExceptionContextLoggingData loggingData = this.formatter.Format(context);

            this.logger.LogError(loggingData.ToErrorString());
        }
    }
}
