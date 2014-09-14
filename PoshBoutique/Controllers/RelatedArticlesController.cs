using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    public class RelatedArticlesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetRelatedArticles(int id)
        {
            ArticlesProvider articlesProvider = new ArticlesProvider();
            IEnumerable<ArticleModel> relatedArticles = await articlesProvider.GetRelatedArticles(id);
            if (relatedArticles == null)
            {
                return this.NotFound();
            }

            return this.Ok(relatedArticles);
        }
    }
}
