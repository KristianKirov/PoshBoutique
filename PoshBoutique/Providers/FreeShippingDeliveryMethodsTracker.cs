using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Providers
{
    public class FreeShippingDeliveryMethodsTracker : IModelTracker<DeliveryMethodModel>
    {
        public void TrackItemCreated(DeliveryMethodModel item)
        {
            item.DeliveryPrice = 0;
            item.CODTax = 0;
        }
    }
}