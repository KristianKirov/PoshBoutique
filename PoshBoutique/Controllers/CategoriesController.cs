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
    [RoutePrefix("api/Categories")]
    public class CategoriesController : ApiController
    {
        [Route("Tree")]
        public IEnumerable<CategoryModel> GetTree()
        {
            CategoriesProvider categoriesProvider = new CategoriesProvider();

            return categoriesProvider.GetCategoriesTree();
        }
    }
}
