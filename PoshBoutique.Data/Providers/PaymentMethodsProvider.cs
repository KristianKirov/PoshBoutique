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
        public async Task<IEnumerable<PaymentMethodModel>> GetAllPaymentMethods()
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                return await dataContext.PaymentMethods.OrderBy(d => d.OrderIndex).ThenBy(d => d.Id).Select(d => new PaymentMethodModel()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        LogoUrl = d.LogoUrl,
                        Description = d.Description,
                        ApplyDeliveryTax = d.ApplyDeliveryTax,
                        OrderIndex = d.OrderIndex,
                        IsExternal = d.IsExternal
                    }).ToArrayAsync();
            }
        }

        public async Task<PaymentMethodModel> GetPaymentMethodById(int paymentMethodId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                PaymentMethod foundPaymentMethod = await dataContext.PaymentMethods.FindAsync(paymentMethodId);

                return new PaymentMethodModel()
                {
                    Id = foundPaymentMethod.Id,
                    Name = foundPaymentMethod.Name,
                    LogoUrl = foundPaymentMethod.LogoUrl,
                    Description = foundPaymentMethod.Description,
                    ApplyDeliveryTax = foundPaymentMethod.ApplyDeliveryTax,
                    OrderIndex = foundPaymentMethod.OrderIndex,
                    IsExternal = foundPaymentMethod.IsExternal
                };
            }
        }
    }
}
