using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class OrderItemModel
    {
        public string ArticleTitle { get; set; }

        public string ArticleUrlName { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public decimal ItemPrice { get; set; }

        public int Quantity { get; set; }
    }
}
