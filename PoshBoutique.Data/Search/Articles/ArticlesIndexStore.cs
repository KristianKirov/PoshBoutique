using Lucene.Net.Documents;
using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using PoshBoutique.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Search.Articles
{
    public sealed class ArticlesIndexStore : ISearchEntryAdapter<ArticleModel>
    {
        public static ArticlesIndexStore Current { get; private set; }

        private readonly SearchFileSystemStorage<ArticleModel> indexStorage;

        private readonly string[] searchableFields;

        private ArticlesIndexStore()
        {
            this.indexStorage = new SearchFileSystemStorage<ArticleModel>(this);
            this.searchableFields = new string[] { "Title", "Description", "MaterialDescription", "ShortDescription" };
        }

        static ArticlesIndexStore()
        {
            ArticlesIndexStore.Current = new ArticlesIndexStore();
        }

        public string UniqueKey
        {
            get
            {
                return "Articles";
            }
        }

        public IEnumerable<ArticleModel> AssembleItems(IEnumerable<Document> indexDocuments)
        {
            Dictionary<int, int> articlesOrderMap = new Dictionary<int, int>();
            int documentIndex = 0;
            foreach (Document document in indexDocuments)
            {
                articlesOrderMap.Add(int.Parse(document.Get("Id")), documentIndex++);
            }

            IEnumerable<int> articleIds = articlesOrderMap.Keys;
            ArticlesProvider articlesProvider = new ArticlesProvider();
            IEnumerable<ArticleModel> foundArticles = articlesProvider.GetArticlesByIds(articleIds).Result.OrderBy(a => articlesOrderMap[a.Id]).ToList();

            return foundArticles;
        }

        public string[] GetSearchFieldNames()
        {
            return this.searchableFields;
        }

        public void UpsertItems(IEnumerable<Article> articles)
        {
            ArticleSearchEntry[] searchEntries = articles.Select(a => new ArticleSearchEntry(a)).ToArray();

            this.indexStorage.AddUpdateLuceneIndex(searchEntries);
        }

        public void UpsertItem(Article article)
        {
            this.UpsertItems(new Article[1] { article });
        }

        public bool Clear()
        {
            return this.indexStorage.ClearLuceneIndex();
        }

        public void Optimize()
        {
            this.indexStorage.Optimize();
        }

        public void RemoveItem(int id)
        {
            this.indexStorage.ClearLuceneIndexRecord(new IndexField("Id", id.ToString()));
        }

        public IEnumerable<ArticleModel> Find(string searchTerm)
        {
            return this.indexStorage.Search(searchTerm);
        }
    }
}
