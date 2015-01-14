using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Templates
{
    public interface ITemplateProvider
    {
        string GetTemplate(string name);
    }
}
