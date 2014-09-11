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
    public class FeedbackController : ApiController
    {
        public async Task<IHttpActionResult> Submit(FeedbackModel feedback)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            FeedbackSubmissionsProvider feedbackProvider = new FeedbackSubmissionsProvider();
            await feedbackProvider.CreateFeedbackSubmission(feedback.Email, feedback.Name, feedback.Message);

            return this.Ok();
        }
    }
}
