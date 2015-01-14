using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class StockChangeModel
    {
        public int ArticleId { get; private set; }

        public int SizeId { get; private set; }

        public int ColorId { get; private set; }

        public int QuantityChange { get; private set; }

        public StockChangeModel(int articleId, int sizeId, int? colorId, int quantityChange)
        {
            this.ArticleId = articleId;
            this.SizeId = sizeId;
            this.ColorId = colorId == null ? 0 : colorId.Value;
            this.QuantityChange = quantityChange;
        }
    }
}
