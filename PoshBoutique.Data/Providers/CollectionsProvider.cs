using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class CollectionsProvider
    {
        public async Task<IEnumerable<CollectionModel>> GetAllCollections()
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                IEnumerable<CollectionModel> collection = await dataContext.Collections.OrderByDescending(c => c.DateCreated).Select(c => new CollectionModel()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImageUrl = c.ImageUrl,
                        DateCreated = c.DateCreated
                    }).ToListAsync();

                return collection;
            }
        }
    }
}
