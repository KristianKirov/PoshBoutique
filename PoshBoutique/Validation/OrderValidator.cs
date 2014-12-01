using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using PoshBoutique.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.Validation
{
    public class OrderValidator
    {
        public async Task<bool> ValidateOrderItems(IEnumerable<OrderedItemModel> orderedItems)
        {
            List<Task<bool>> validationTasks = new List<Task<bool>>();
            foreach (OrderedItemModel orderedItem in orderedItems)
            {
                validationTasks.Add(this.ValidateOrderedItem(orderedItem));   
            }

            bool isValid = true;
            foreach (Task<bool> validationTask in validationTasks)
            {
                bool taskResult = await validationTask;
                isValid &= taskResult;
            }

            return isValid;
        }

        public async Task<bool> ValidateOrderedItem(OrderedItemModel orderedItem)
        {
            ArticlesProvider articlesProvider = new ArticlesProvider();
            Task<bool> articleExistsTask = articlesProvider.HasArticleWithPrice(orderedItem.ArticleId.Value, orderedItem.Price.Value);

            StocksProvider stocksProvider = new StocksProvider();
            Task<bool> hasStocksTask = stocksProvider.HasEnoughStocksOfArticle(orderedItem.ArticleId.Value, orderedItem.SizeId.Value, orderedItem.ColorId, orderedItem.Quantity.Value);

            bool articleExists = await articleExistsTask;
            bool hasEnoughStocks = await hasStocksTask;

            bool isOrderValid = articleExists && hasEnoughStocks;

            return isOrderValid;
        }

        public async Task<bool> ValidatePrices(FullOrderModel order)
        {
            decimal priceAccuracy = 0.1m;
            DeliveryMethodsProvider deliveryMethodsProvider = new DeliveryMethodsProvider();
            Task<DeliveryMethodModel> getDeliveryMethodTask = deliveryMethodsProvider.GetDeliveryMethodById(order.DeliveryMethodId.Value);

            PaymentMethodsProvider paymentMethodsProvider = new PaymentMethodsProvider();
            Task<PaymentMethodModel> getPaymentMethodTask = paymentMethodsProvider.GetPaymentMethodById(order.PaymentMethodId.Value);

            decimal itemsPrice = order.Items.Sum(i => i.Quantity.Value * i.Price.Value);
            if (Math.Abs(itemsPrice - order.Total.Order.Value) > priceAccuracy)
            {
                return false;
            }

            DeliveryMethodModel deliveryMethod = await getDeliveryMethodTask;
            PaymentMethodModel paymentMethod = await getPaymentMethodTask;

            decimal shippingPrice = deliveryMethod.DeliveryPrice;
            if (paymentMethod.ApplyDeliveryTax)
            {
                shippingPrice += ((decimal)deliveryMethod.CODTax * order.Total.Order.Value) / 100m;
            }

            if (Math.Abs(shippingPrice - order.Total.Shipping.Value) > priceAccuracy)
            {
                return false;
            }

            decimal totalPrice = shippingPrice + itemsPrice;
            if (Math.Abs(totalPrice - order.Total.Full.Value) > priceAccuracy)
            {
                return false;
            }

            return true;
        }
    }
}