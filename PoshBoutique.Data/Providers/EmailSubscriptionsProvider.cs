using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class EmailSubscriptionsProvider
    {
        public async Task<bool> SubscribeEmail(string email)
        {
            if (await this.IsEmailSubscribed(email))
            {
                return false;
            }

            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                dataContext.EmailSubscriptions.Add(new EmailSubscription()
                {
                    Email = email
                });

                await dataContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> IsEmailSubscribed(string email)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                return await dataContext.EmailSubscriptions.AnyAsync(s => s.Email == email);
            }
        }
    }
}
