using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Common.Logging.Mvc.Formatting
{
    public class ExceptionLoggerContextFormatter : IExceptionLoggerContextFormatter
    {
        // According to: http://www.asp.net/web-api/overview/error-handling/web-api-global-error-handling
        // Exception, Request and RequestContext are always provided (not null) except in unit tests
        public ExceptionContextLoggingData Format(ExceptionLoggerContext context)
        {
            ExceptionContextLoggingData loggingData = new ExceptionContextLoggingData(context.Exception.ToString());

            loggingData.ExtendedProperties.Add("Request.Uri", context.Request.RequestUri.ToString());
            loggingData.ExtendedProperties.Add("Request.Method", context.Request.Method.Method);

            IPrincipal currentPrincipal = context.RequestContext.Principal;
            if (currentPrincipal != null && currentPrincipal.Identity != null)
            {
                loggingData.ExtendedProperties.Add("RequestContext.Authenticated", context.RequestContext.Principal.Identity.IsAuthenticated.ToString());
                loggingData.ExtendedProperties.Add("RequestContext.Identity", context.RequestContext.Principal.Identity.Name);
            }
            loggingData.ExtendedProperties.Add("RequestContext.VirtualPathRoot", context.RequestContext.VirtualPathRoot);

            if (context.CatchBlock != null)
            {
                loggingData.ExtendedProperties.Add("CatchBlock.Name", context.CatchBlock.Name);
                loggingData.ExtendedProperties.Add("CatchBlock.IsTopLevel", context.CatchBlock.IsTopLevel.ToString());
            }

            return loggingData;
        }
    }
}
