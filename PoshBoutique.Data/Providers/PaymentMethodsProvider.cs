using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class PaymentMethodsProvider
    {
        private IModelTracker<PaymentMethodModel> paymentMethodsTracker;

        public PaymentMethodsProvider(IModelTracker<PaymentMethodModel> paymentMethodsTracker)
        {
            this.paymentMethodsTracker = paymentMethodsTracker;
        }

        private PaymentMethodModel CreatePaymentMethodModel(PaymentMethod paymentMethod)
        {
            PaymentMethodModel model = new PaymentMethodModel()
            {
                Id = paymentMethod.Id,
                Name = paymentMethod.Name,
                LogoUrl = paymentMethod.LogoUrl,
                Description = paymentMethod.Description,
                ApplyDeliveryTax = paymentMethod.ApplyDeliveryTax,
                OrderIndex = paymentMethod.OrderIndex,
                IsExternal = paymentMethod.IsExternal
            };

            this.paymentMethodsTracker.TrackItemCreated(model);

            return model;
        }

        public async Task<IEnumerable<PaymentMethodModel>> GetAllPaymentMethods()
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                PaymentMethod[] paymentMethods = await dataContext.PaymentMethods.OrderBy(d => d.OrderIndex).ThenBy(d => d.Id).ToArrayAsync();
                return paymentMethods.Select(d => this.CreatePaymentMethodModel(d)).ToArray();
            }
        }

        public async Task<PaymentMethodModel> GetPaymentMethodById(int paymentMethodId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                PaymentMethod foundPaymentMethod = await dataContext.PaymentMethods.FindAsync(paymentMethodId);

                return this.CreatePaymentMethodModel(foundPaymentMethod);
            }
        }
    }
}
