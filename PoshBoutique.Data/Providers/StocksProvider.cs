using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PoshBoutique.Data.Models;
using Common.Logging.Model;

namespace PoshBoutique.Data.Providers
{
    public class StocksProvider
    {
        public async Task<bool> HasEnoughStocksOfArticle(int articleId, int sizeId, int? colorId, int quantity)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                int colorIdValue = colorId == null ? 0 : colorId.Value;
                bool hasEnoughStocks = await dataContext.Stocks.AnyAsync(s => s.ArticleId == articleId && s.SizeId == sizeId && s.ColorId == colorIdValue && s.Quantity >= quantity);

                return hasEnoughStocks;
            }
        }

        public async Task<bool> UpdateStocks(IEnumerable<StockChangeModel> stockChanges, ILogger logger)
        {
            if (stockChanges == null || !stockChanges.Any())
            {
                return true;
            }

            bool result = false;

            try
            {
                using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
                {
                    using (DbContextTransaction transaction = dataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (StockChangeModel stockChange in stockChanges)
                            {
                                int stocksUpdatedCount = await dataContext.Database.ExecuteSqlCommandAsync(
                                    "UPDATE Stocks SET Quantity = Quantity + @p3 WHERE ArticleId = @p0 and SizeId = @p1 and ColorId = @p2",
                                    stockChange.ArticleId,
                                    stockChange.SizeId,
                                    stockChange.ColorId,
                                    stockChange.QuantityChange);

                                if (stocksUpdatedCount != 1)
                                {
                                    throw new InvalidOperationException(
                                        string.Format("Invalid updated stocks count: {0}. Change model: a: {1}, s: {2}, c: {3}, q: {4}",
                                            stocksUpdatedCount,
                                            stockChange.ArticleId,
                                            stockChange.SizeId,
                                            stockChange.ColorId,
                                            stockChange.QuantityChange));
                                }
                            }

                            transaction.Commit();

                            result = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            result = false;

                            logger.LogError(ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;

                logger.LogError(ex.ToString());
            }

            return result;
        }
    }
}
