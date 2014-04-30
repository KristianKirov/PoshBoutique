using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    public class TestController : ApiController
    {
        public IHttpActionResult Public()
        {
            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult Auth([FromBody]string userName, string password)
        {
            if (userName == "Gancho" && password == "Gankin")
            {
                return this.Json<object>(new { token = "123" });
            }

            return this.StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        public IHttpActionResult Private()
        {
            if (this.Request.Headers.Authorization == null || this.Request.Headers.Authorization.Parameter != "123")
            {
                return this.StatusCode(HttpStatusCode.Unauthorized);
            }

            return this.Json<object>(new { Address = "Sofia" });
        }
    }
}