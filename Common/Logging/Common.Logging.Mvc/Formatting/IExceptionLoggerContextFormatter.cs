using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Common.Logging.Mvc.Formatting
{
    public interface IExceptionLoggerContextFormatter
    {
        ExceptionContextLoggingData Format(ExceptionLoggerContext context);
    }
}
