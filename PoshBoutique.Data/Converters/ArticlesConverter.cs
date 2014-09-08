using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Converters
{
    public class ArticlesConverter
    {
        public ArticleModel ToModel(Article article)
        {
            return new ArticleModel()
            {
                Id = article.Id,
                UrlName = article.UrlName,
                Title = article.Title,
                DateCreated = article.DateCreated,
                Price = article.Price,
                Description = article.Description,
                ShortDescription = article.ShortDescription,
                MaterialDescription = article.MaterialDescription,
                ThumbnailUrl = article.ThumbnailUrl,
                SizeTypeId = article.SizeTypeId,
                Visible = article.Visible,
                OriginalPrice = article.OriginalPrice,
                DiscountDescription = article.DiscountDescription,
                HasDiscount = article.OriginalPrice != null,
                LikesCount = article.LikesCount,
                OrdersCount = article.OrdersCount
            };
        }

        public FullArticleModel ToFullModel(Article article)
        {
            return new FullArticleModel();
        }
    }
}
