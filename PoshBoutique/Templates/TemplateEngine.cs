using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Templates
{
    public class TemplateEngine
    {
        private ITemplateProvider templateProvider;

        private ITemplateParser templateParser;

        public TemplateEngine(ITemplateProvider templateProvider, ITemplateParser templateParser)
        {
            this.templateProvider = templateProvider;
            this.templateParser = templateParser;
        }

        public string RenderTemplate(string templateName, object templateData)
        {
            string template = this.templateProvider.GetTemplate(templateName);

            string renderedTemplate = this.templateParser.ParseTemplate(template, templateData);

            return renderedTemplate;
        }
    }
}