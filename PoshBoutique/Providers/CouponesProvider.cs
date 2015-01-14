using PoshBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.Providers
{
    public class CouponesProvider
    {
        internal static readonly CouponeModel LoyalCustomerCoupone;

        static CouponesProvider()
        {
            CouponesProvider.LoyalCustomerCoupone = new CouponeModel()
            {
                Name = "Loyal Customer",
                Value = 10,
                ValueType = CouponeValueType.Percent,
                Description = "10% намаление на всеки ненамален продукт",
                FreeShipping = true
            };
        }

        public Task<IEnumerable<CouponeModel>> GetCoupons(bool includeAuthenticated, bool includeLoyalCustomer)
        {
            List<CouponeModel> coupons = new List<CouponeModel>();
            if (includeAuthenticated && includeLoyalCustomer)
            {
                coupons.Add(CouponesProvider.LoyalCustomerCoupone);
            }

            return Task.FromResult<IEnumerable<CouponeModel>>(coupons);
        }

        public Task<bool> CouponeExists(string name, int value, Models.CouponeValueType couponeValueType, bool freeShipping)
        {
            if (name == CouponesProvider.LoyalCustomerCoupone.Name)
            {
                bool isValidLoyalCustomerCoupone = value == CouponesProvider.LoyalCustomerCoupone.Value && couponeValueType == CouponesProvider.LoyalCustomerCoupone.ValueType && freeShipping == CouponesProvider.LoyalCustomerCoupone.FreeShipping;

                return Task.FromResult<bool>(isValidLoyalCustomerCoupone);
            }

            return Task.FromResult<bool>(false);
        }
    }
}