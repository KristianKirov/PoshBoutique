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
        public async Task<bool> ValidateOrder(IEnumerable<OrderedItemModel> orderedItems)
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
    }
}