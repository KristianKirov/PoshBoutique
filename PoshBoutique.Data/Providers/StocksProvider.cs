using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

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
    }
}
