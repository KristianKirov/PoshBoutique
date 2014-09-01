using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    public class ArticlesController : ApiController
    {
        public IHttpActionResult Get(string categoryUrl, string orderBy, SortDirection sortDirection, string filter = null)
        {
            ArticlesProvider articlesProvider = new ArticlesProvider();

            ArticlesListModel articlesList = articlesProvider.GetArticlesInCategory(categoryUrl, filter, orderBy, sortDirection);
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
    }
}
