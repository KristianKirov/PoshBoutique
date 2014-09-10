using PoshBoutique.Data.Providers;
using PoshBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    public class SubscriptionsController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> Subscribe([FromBody]EmailSubscriptionModel emailModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid email");
            }

            EmailSubscriptionsProvider subscriptionsProvider = new EmailSubscriptionsProvider();
            bool emailSubscribed = await subscriptionsProvider.SubscribeEmail(emailModel.Email);

            if (!emailSubscribed)
            {
                return this.Conflict();
            }

            return this.Ok();
        }
    }
}
