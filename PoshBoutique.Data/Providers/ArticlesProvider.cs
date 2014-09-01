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
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Category category = dataContext.Categories.FirstOrDefault(c => c.UrlName == categoryUrl);
                if (category == null && !categoryUrl.Equals(ArticlesProvider.ALL_CATEGORIES_URL_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                articlesListModel = new ArticlesListModel();
                articlesListModel.Category = new CategoriesConverter().ToModel(category);

                IQueryable<Article> articlesQuery = dataContext.Articles
                    .Where(article => article.Visible);
                if (category != null)
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

            return articlesListModel;
        }

        public FullArticleModel GetFullArticleByUrlName(string urlName)
        {
            FullArticleModel articleModel = null;
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                string l = string.Empty;

                dataContext.Database.Log = s => l += s;

                Article article = dataContext.Articles
                    .Include(a => a.ArticleImages)
                    .Include(a => a.Stocks)
                    .Include(a => a.Stocks.Select(s => s.Color))
                    .Include(a => a.Stocks.Select(s => s.Size))
                    .FirstOrDefault(a => a.UrlName == urlName);

                articleModel = new FullArticleModel()
                {
                    Title = article.Title,
                    Description = article.Description,
                    MaterialDescription = article.MaterialDescription,
                    Price = article.Price
                };

                Dictionary<int, SizeModel> sizesDictionary = article.Stocks.Select(s => s.Size).Distinct().ToDictionary(s => s.Id, s => new SizeModel()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        OrderIndex = s.OrderIndex,
                        Quantity = 0
                    });

                foreach (Stock stock in article.Stocks)
                {
                    SizeModel sizeModel = sizesDictionary[stock.SizeId];
                    sizeModel.AddColor(stock.Color, stock.Quantity);
                }

                articleModel.Sizes = sizesDictionary.Values.ToList().OrderBy(s => s.OrderIndex);
                articleModel.Images = article.ArticleImages.OrderBy(i => i.OrderIndex).Select(i => new ImageModel()
                    {
                        SmallUrl = i.SmallImageUrl,
                        MediumUrl = i.MediumImageUrl,
                        LargeUrl = i.LargelImageUrl
                    }).ToList();
            }

            return articleModel;
        }
    }
}
