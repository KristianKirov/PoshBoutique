using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoshBoutique.Data.Extensions;
using PoshBoutique.Data.Converters;

namespace PoshBoutique.Data.Providers
{
    public class ArticlesProvider
    {
        private const string ALL_CATEGORIES_URL_NAME = "all";

        public ArticlesListModel GetArticlesInCategory(string categoryUrl, string filter, string orderBy, SortDirection sortDirection)
        {
            ArticlesListModel articlesListModel = null;
            using (Entities dataContext = new Entities())
            {
                Category category = dataContext.Categories.FirstOrDefault(c => c.UrlName == categoryUrl);
                if (category != null)
                {
                    articlesListModel = new ArticlesListModel();
                    articlesListModel.Category = new CategoriesConverter().ToModel(category);

                    IQueryable<Article> articlesQuery = dataContext.Articles
                        .Where(article => article.Visible);
                    if (!categoryUrl.Equals(ArticlesProvider.ALL_CATEGORIES_URL_NAME, StringComparison.InvariantCultureIgnoreCase))
                    {
                        articlesQuery = articlesQuery.Where(article => article.Categories.Any(c => c.Id == category.Id));
                    }

                    if (!string.IsNullOrEmpty(filter))
                    {
                        articlesQuery = articlesQuery
                            .Where(article => article.Title.Contains(filter) || article.Description.Contains(filter) || article.MaterialDescription.Contains(filter));
                    }

                    articlesQuery = articlesQuery.Sort(orderBy, sortDirection);

                    ArticlesConverter converter = new ArticlesConverter();
                    articlesListModel.Articles = articlesQuery.ToList().Select(a => converter.ToModel(a)).ToList();
                }
            }

            return articlesListModel;
        }

        public FullArticleModel GetFullArticleByUrlName(string urlName)
        {
            FullArticleModel articleModel = null;
            using (Entities dataContext = new Entities())
            {
                string l = string.Empty;

                dataContext.Database.Log = s => l += s;

                Article article = dataContext.Articles
                    .Include(a => a.ArticleImages)
                    .Include(a => a.Stocks)
                    .Include(a => a.Stocks.Select(s => s.Color))
                    .Include(a => a.Stocks.Select(s => s.Size))
                    .FirstOrDefault(a => a.UrlName == urlName);
            }

            return articleModel;
        }
    }
}
