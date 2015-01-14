using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Providers
{
    public class FreeShippingPaymentMethodsTracker : IModelTracker<PaymentMethodModel>
    {
        public void TrackItemCreated(PaymentMethodModel item)
        {
            item.ApplyDeliveryTax = false;
        }
    }
}