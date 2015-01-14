using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PoshBoutique.Templates
{
    public static class TemplateEngineFactory
    {
        public static TemplateEngine GetDefault()
        {
            string templatesPath = HostingEnvironment.MapPath("~/templates/emails");
            ITemplateProvider templateProvider = new FileSystemTemplateProvider(templatesPath);
            ITemplateParser templateParser = new MustacheTemplateParser();

            return new TemplateEngine(templateProvider, templateParser);
        }
    }
}