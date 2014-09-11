using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Providers
{
    public class FeedbackSubmissionsProvider
    {
        public async Task CreateFeedbackSubmission(string email, string name, string message)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                dataContext.FeedbackSubmissions.Add(new FeedbackSubmission()
                {
                    Email = email,
                    Name = name,
                    Message = message
                });

                await dataContext.SaveChangesAsync();
            }
        }
    }
}
