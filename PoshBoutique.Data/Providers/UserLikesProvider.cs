using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class UserLikesProvider
    {
        public async Task<bool> RegisterLike(Guid userId, int articleId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                bool isLikeRegistered = await dataContext.UserLikes.AnyAsync(ul => ul.UserId == userId && ul.ArticleId == articleId);
                if (isLikeRegistered)
                {
                    return false;
                }

                dataContext.UserLikes.Add(new UserLike()
                {
                    UserId = userId,
                    ArticleId = articleId
                });

                await dataContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> UnregisterLike(Guid userId, int articleId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                UserLike registeredLike = await dataContext.UserLikes.FirstAsync(ul => ul.UserId == userId && ul.ArticleId == articleId);
                if (registeredLike == null)
                {
                    return false;
                }

                dataContext.UserLikes.Remove(registeredLike);

                await dataContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<HashSet<int>> GetLikedArticlesByUser(Guid userId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                List<int> userLikesIds = dataContext.UserLikes.Where(ul => ul.UserId == userId).Select(ul => ul.ArticleId).ToList();
                if (userLikesIds.Count == 0)
                {
                    return null;
                }

                HashSet<int> userLikesSet = new HashSet<int>(userLikesIds);
                return userLikesSet;
            }
        }
    }
}
