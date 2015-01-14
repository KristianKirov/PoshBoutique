using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PoshBoutique.Templates
{
    public class FileSystemTemplateProvider : ITemplateProvider
    {
        private string templatesDirectory;

        public FileSystemTemplateProvider(string templatesDirectory)
        {
            this.templatesDirectory = templatesDirectory;
        }

        public string GetTemplate(string name)
        {
            string templatePath = Path.Combine(this.templatesDirectory, string.Concat(name, ".html"));

            return File.ReadAllText(templatePath);
        }
    }
}