using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search
{
    public interface ISearchEntry
    {
        string KeyField { get; }
        string KeyValue { get; }

        IEnumerable<SearchFieldData> SearchFieldsData { get; }
    }
}
