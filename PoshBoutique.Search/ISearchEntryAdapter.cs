using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search
{
    public interface ISearchEntryAdapter<T>
    {
        string UniqueKey { get; }
        IEnumerable<T> AssembleItems(IEnumerable<Document> indexDocuments);
        string[] GetSearchFieldNames();
    }
}
