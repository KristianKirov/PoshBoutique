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
    public class DeliveryMethodsTrackerFactory
    {
        public IModelTracker<DeliveryMethodModel> GetDeliveryMethodsTracker(bool freeShipping)
        {
            if (!freeShipping)
            {
                return new HollowMethodsTracker<DeliveryMethodModel>();
            }

            return new FreeShippingDeliveryMethodsTracker();
        }
    }
}