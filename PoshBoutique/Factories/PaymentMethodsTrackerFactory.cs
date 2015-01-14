using PoshBoutique.Data.Models;
using PoshBoutique.Identity;
using PoshBoutique.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.Factories
{
    public class PaymentMethodsTrackerFactory
    {
        public IModelTracker<PaymentMethodModel> GetPaymentMethodsTracker(bool freeShipping)
        {
            if (!freeShipping)
            {
                return new HollowMethodsTracker<PaymentMethodModel>();
            }

            return new FreeShippingPaymentMethodsTracker();
        }
    }
}