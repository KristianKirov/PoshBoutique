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
    public class CollectionsController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetAllCollections()
        {
            CollectionsProvider collectionsProvider = new CollectionsProvider();
            IEnumerable<CollectionModel> allCollections = await collectionsProvider.GetAllCollections();

            return this.Ok(allCollections);
        }
    }
}
