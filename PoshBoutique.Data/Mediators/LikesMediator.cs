using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Mediators
{
    public class LikesMediator
    {
        private readonly ArticlesProvider articlesProvider;
        private readonly UserLikesProvider userLikesProvider;

        public LikesMediator() : this (new ArticlesProvider(), new UserLikesProvider())
        {
        }

        public LikesMediator(ArticlesProvider articlesProvider, UserLikesProvider userLikesProvider)
        {
            this.articlesProvider = articlesProvider;
            this.userLikesProvider = userLikesProvider;
        }

        public async Task<bool> LikeArticle(Guid? userId, int articleId)
        {
            bool articleExists = await this.articlesProvider.ArticleExists(articleId);
            if (!articleExists)
            {
                return false;
            }

            if (userId != null && userId != Guid.Empty)
            {
                bool userLikeRegistered = await this.userLikesProvider.RegisterLike(userId.Value, articleId);
                if (!userLikeRegistered)
                {
                    return false;
                }
            }

            await this.articlesProvider.LikeArticle(articleId);

            return true;
        }

        public async Task<bool> UnlikeArticle(Guid? userId, int articleId)
        {
            if (userId == null)
            {
                return false;
            }

            //Likes count of the article is not decreased because of marketing purposes
            return await this.userLikesProvider.UnregisterLike(userId.Value, articleId);
        }
    }
}
