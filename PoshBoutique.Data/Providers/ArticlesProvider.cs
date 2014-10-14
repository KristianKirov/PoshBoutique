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

        public async Task<ArticlesListModel> GetArticlesInCategory(string categoryUrl, string filter, string orderBy, SortDirection sortDirection, Guid? currentUserId)
        {
            ArticlesListModel articlesListModel = null;
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Category category = await dataContext.Categories.FirstOrDefaultAsync(c => c.UrlName == categoryUrl);
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

                if (!string.IsNullOrEmpty(orderBy))
                {
                    articlesQuery = articlesQuery.Sort(orderBy, sortDirection);
                }

                List<Article> articlesList = await articlesQuery.ToListAsync();
                HashSet<int> userLikes = this.GetUserLikes(currentUserId);

                ArticlesConverter converter = new ArticlesConverter();
                articlesListModel.Articles = articlesList.Select(a => converter.ToModel(a, userLikes)).ToList();
            }

            return articlesListModel;
        }

        private HashSet<int> GetUserLikes(Guid? currentUserId)
        {
            HashSet<int> userLikes = null;
            if (currentUserId != null)
            {
                UserLikesProvider userLikesProvider = new UserLikesProvider();
                userLikes = userLikesProvider.GetLikedArticlesByUser(currentUserId.Value);
            }

            return userLikes;
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
                    Id = article.Id,
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

        public async Task<bool> LikeArticle(int articleId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article likedArticle = await dataContext.Articles.FindAsync(articleId);
                if (likedArticle == null)
                {
                    return false;
                }

                likedArticle.LikesCount++;

                await dataContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> ArticleExists(int articleId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                return await dataContext.Articles.AnyAsync(a => a.Id == articleId);
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetRelatedArticles(int articleId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article parentArticle = await dataContext.Articles.FindAsync(articleId);
                if (parentArticle == null)
                {
                    return null;
                }

                Article[] relatedArticles = parentArticle.RelatedArticles.OrderByDescending(a => a.DateCreated).ToArray();
                if (relatedArticles.Length == 0)
                {
                    return null;
                }

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] relatedArticlesModels = relatedArticles.Select(a => converter.ToModel(a, null)).ToArray();

                return relatedArticlesModels;
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesInCollection(int collectionId, Guid? currentUserId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article[] articlesInCollection = await dataContext.Articles.Where(a => a.CollectionId == collectionId && a.Visible).OrderByDescending(a => a.DateCreated).ToArrayAsync();
                if (articlesInCollection.Length == 0)
                {
                    return null;
                }

                HashSet<int> userLikes = this.GetUserLikes(currentUserId);

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] articlesInCollectionModels = articlesInCollection.Select(a => converter.ToModel(a, userLikes)).ToArray();

                return articlesInCollectionModels;
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetDiscountedArticles(Guid? currentUserId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article[] discountedArticles = await dataContext.Articles.Where(a => a.OriginalPrice != null && a.Visible).OrderByDescending(a => a.DateCreated).ToArrayAsync();
                if (discountedArticles.Length == 0)
                {
                    return null;
                }

                HashSet<int> userLikes = this.GetUserLikes(currentUserId);

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] discountedArticlesModels = discountedArticles.Select(a => converter.ToModel(a, userLikes)).ToArray();

                return discountedArticlesModels;
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetFeaturedArticles(Guid? currentUserId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article[] featuredArticles = await dataContext.Articles.Where(a => a.IsFeatured && a.Visible).OrderByDescending(a => a.DateCreated).ToArrayAsync();
                if (featuredArticles.Length == 0)
                {
                    return null;
                }

                HashSet<int> userLikes = this.GetUserLikes(currentUserId);

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] featuredArticlesModels = featuredArticles.Select(a => converter.ToModel(a, userLikes)).ToArray();

                return featuredArticlesModels;
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetLikedArticles(Guid userId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                IQueryable<UserLike> userLikesQuery = dataContext.UserLikes.Where(ul => ul.UserId == userId);
                Article[] likedArticles = await dataContext.Articles.Join(userLikesQuery, a => a.Id, ul => ul.ArticleId, (a, ul) => a).OrderBy(a => a.Title).ToArrayAsync();

                if (likedArticles.Length == 0)
                {
                    return null;
                }

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] likedArticlesModels = likedArticles.Select(a => 
                    {
                        ArticleModel likedArticleModel = converter.ToModel(a, null);
                        likedArticleModel.IsLiked = true;

                        return likedArticleModel;
                    }).ToArray();

                return likedArticlesModels;
            }
        }

        public IEnumerable<ArticleModel> GetArticlesByIds(IEnumerable<int> articleIds)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Article[] articlesInCollection = dataContext.Articles.Where(a => articleIds.Contains(a.Id) && a.Visible).OrderByDescending(a => a.DateCreated).ToArray();
                if (articlesInCollection.Length == 0)
                {
                    return null;
                }

                ArticlesConverter converter = new ArticlesConverter();
                ArticleModel[] articlesInCollectionModels = articlesInCollection.Select(a => converter.ToModel(a, null)).ToArray();

                return articlesInCollectionModels;
            }
        }
    }
}
