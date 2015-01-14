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
        private IModelTracker<DeliveryMethodModel> deliveryMethodsTracker;

        public DeliveryMethodsProvider(IModelTracker<DeliveryMethodModel> deliveryMethodsTracker)
        {
            this.deliveryMethodsTracker = deliveryMethodsTracker;
        }

        private DeliveryMethodModel CreateDeliveryMethodModel(DeliveryMethod deliveryMethod)
        {
            DeliveryMethodModel model = new DeliveryMethodModel()
            {
                Id = deliveryMethod.Id,
                Name = deliveryMethod.Name,
                LogoUrl = deliveryMethod.LogoUrl,
                Description = deliveryMethod.Description,
                DeliveryPrice = deliveryMethod.DeliveryPrice,
                CODTax = deliveryMethod.CODTax,
                OrderIndex = deliveryMethod.OrderIndex
            };

            this.deliveryMethodsTracker.TrackItemCreated(model);

            return model;
        }

        public async Task<IEnumerable<DeliveryMethodModel>> GetAllDeliveryMethods()
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                DeliveryMethod[] deliveryMethods = await dataContext.DeliveryMethods.OrderBy(d => d.OrderIndex).ThenBy(d => d.Id).ToArrayAsync();
                return deliveryMethods.Select(d => this.CreateDeliveryMethodModel(d)).ToArray();
            }
        }

        public async Task<DeliveryMethodModel> GetDeliveryMethodById(int deliveryMethodId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                DeliveryMethod foundDeliveryMethod = await dataContext.DeliveryMethods.FindAsync(deliveryMethodId);

                return this.CreateDeliveryMethodModel(foundDeliveryMethod);
            }
        }
    }
}
