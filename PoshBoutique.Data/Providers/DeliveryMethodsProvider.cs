using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class DeliveryMethodsProvider
    {
        public async Task<IEnumerable<DeliveryMethodModel>> GetAllDeliveryMethods()
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                return await dataContext.DeliveryMethods.OrderBy(d => d.OrderIndex).ThenBy(d => d.Id).Select(d => new DeliveryMethodModel()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        LogoUrl = d.LogoUrl,
                        Description = d.Description,
                        DeliveryPrice = d.DeliveryPrice,
                        CODTax = d.CODTax,
                        OrderIndex = d.OrderIndex
                    }).ToArrayAsync();
            }
        }
    }
}
