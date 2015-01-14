using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging.Mvc.Formatting
{
    public class ExceptionContextLoggingData
    {
        public string Message { get; private set; }

        public IDictionary<string, string> ExtendedProperties { get; private set; }

        public ExceptionContextLoggingData(string message)
        {
            this.Message = message;
            this.ExtendedProperties = new Dictionary<string, string>();
        }

        public string ToErrorString()
        {
            StringBuilder errorMessageBuilder = new StringBuilder();
            errorMessageBuilder.AppendLine(this.Message);
            foreach (KeyValuePair<string, string> extendedProperty in this.ExtendedProperties)
            {
                errorMessageBuilder.AppendLine(string.Format("{0}: {1}", extendedProperty.Key, extendedProperty.Value));
            }

            return errorMessageBuilder.ToString();
        }
    }
}
