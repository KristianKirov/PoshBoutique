using PoshBoutique.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Search.Articles
{
    internal class ArticleSearchEntry : ISearchEntry
    {
        public IndexField Key { get; private set; }

        private IList<IndexField> searchFieldsData;
        public IEnumerable<IndexField> SearchFieldsData
        {
            get
            {
                return this.searchFieldsData;
            }
        }

        public ArticleSearchEntry(Article article)
        {
            this.Key = new IndexField("Id", article.Id.ToString(), true, false);

            this.searchFieldsData = new List<IndexField>();
            this.searchFieldsData.Add(new IndexField("Title", article.Title, false, true));
            this.searchFieldsData.Add(new IndexField("Description", article.Description, false, true));
            this.searchFieldsData.Add(new IndexField("MaterialDescription", article.MaterialDescription, false, true));
            this.searchFieldsData.Add(new IndexField("ShortDescription", article.ShortDescription, false, true));
        }
    }
}
