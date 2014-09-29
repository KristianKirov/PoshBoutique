using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace PoshBoutique.Controllers
{
    [RoutePrefix("api/Articles")]
    public class ArticlesController : ApiController
    {
        public async Task<IHttpActionResult> Get(string categoryUrl, string orderBy, SortDirection sortDirection, string filter = null)
        {
            Guid? currentUserId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                currentUserId = new Guid(this.User.Identity.GetUserId());
            }

            ArticlesProvider articlesProvider = new ArticlesProvider();
            ArticlesListModel articlesList = await articlesProvider.GetArticlesInCategory(categoryUrl, filter, orderBy, sortDirection, currentUserId);
            if (articlesList == null)
            {
                return this.NotFound();
            }

            return this.Ok(articlesList);
        }

        public IHttpActionResult Get(string urlName)
        {
            ArticlesProvider articlesProvider = new ArticlesProvider();

            FullArticleModel articleModel = articlesProvider.GetFullArticleByUrlName(urlName);
            if (articleModel == null)
            {
                return this.NotFound();
            }

            return this.Ok(articleModel);
        }

        public async Task<IHttpActionResult> Get(int collectionId)
        {
            Guid? currentUserId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                currentUserId = new Guid(this.User.Identity.GetUserId());
            }

            ArticlesProvider articlesProvider = new ArticlesProvider();
            IEnumerable<ArticleModel> articlesInCollection = await articlesProvider.GetArticlesInCollection(collectionId, currentUserId);

            if (articlesInCollection == null) 
            {
                return this.NotFound();
            }

            return this.Ok(articlesInCollection);
        }

        [HttpGet]
        [Route("Discounts")]
        public async Task<IHttpActionResult> GetDiscountedArticles()
        {
            Guid? currentUserId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                currentUserId = new Guid(this.User.Identity.GetUserId());
            }

            ArticlesProvider articlesProvider = new ArticlesProvider();
            IEnumerable<ArticleModel> discountedArticles = await articlesProvider.GetDiscountedArticles(currentUserId);

            if (discountedArticles == null)
            {
                return this.NotFound();
            }

            return this.Ok(discountedArticles);
        }

        [HttpGet]
        [Route("Featured")]
        public async Task<IHttpActionResult> GetFeaturedArticles()
        {
            Guid? currentUserId = null;
            if (this.User.Identity.IsAuthenticated)
            {
                currentUserId = new Guid(this.User.Identity.GetUserId());
            }

            ArticlesProvider articlesProvider = new ArticlesProvider();
            IEnumerable<ArticleModel> featuredArticles = await articlesProvider.GetFeaturedArticles(currentUserId);

            if (featuredArticles == null)
            {
                return this.NotFound();
            }

            return this.Ok(featuredArticles);
        }
    }
}
