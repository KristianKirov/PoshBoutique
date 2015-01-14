using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Templates
{
    public interface ITemplateParser
    {
        string ParseTemplate(string template, object data);
    }
}