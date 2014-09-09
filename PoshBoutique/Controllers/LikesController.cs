using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PoshBoutique.Data.Mediators;
using System.Threading.Tasks;

namespace PoshBoutique.Controllers
{
    public class LikesController : ApiController
    {
        [HttpPut]
        public async Task<IHttpActionResult> Like(int id)
        {
            Guid? currentUserId = null;

            string currentUserIdString = this.User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(currentUserIdString))
            {
                Guid userId;
                Guid.TryParse(currentUserIdString, out userId);
                currentUserId = userId;
            }

            LikesMediator likesMediator = new LikesMediator();
            bool articleLiked = await likesMediator.LikeArticle(currentUserId, id);
            if (!articleLiked)
            {
                return this.BadRequest("Could not like article with id:" + id);
            }

            return this.Ok();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IHttpActionResult> Unlike(int id)
        {
            Guid currentUserId = new Guid(this.User.Identity.GetUserId());
            LikesMediator likesMediator = new LikesMediator();
            bool articleUnliked = await likesMediator.UnlikeArticle(currentUserId, id);
            if (!articleUnliked)
            {
                return this.BadRequest("Could not unlike article with id:" + id);
            }

            return this.Ok();
        }
    }
}