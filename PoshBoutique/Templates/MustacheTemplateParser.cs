using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Templates
{
    public class MustacheTemplateParser : ITemplateParser
    {
        public string ParseTemplate(string template, object data)
        {
            string parsedTemplate = Nustache.Core.Render.StringToString(template, data);

            return parsedTemplate;
        }
    }
}