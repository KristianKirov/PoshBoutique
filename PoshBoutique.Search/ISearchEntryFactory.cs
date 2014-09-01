using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search
{
    public interface ISearchEntryFactory
    {
        ISearchEntry AssembleSearchEntry(IEnumerable<SearchFieldData> SearchFieldsData);
        string[] GetSearchFieldNames();
    }
}
