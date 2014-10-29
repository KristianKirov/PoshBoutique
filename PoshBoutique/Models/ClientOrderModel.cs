using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class ClientOrderModel
    {
        [Required]
        public IEnumerable<OrderedItemModel> Items { get; set; }
    }

    public class OrderedItemModel
    {
        [Required]
        public int? ArticleId { get; set; }

        [Required]
        public int? SizeId { get; set; }

        public int? ColorId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Quantity { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            OrderedItemModel other = obj as OrderedItemModel;
            if (other == null)
            {
                return false;
            }

            if (this.ArticleId != other.ArticleId)
            {
                return false;
            }

            if (this.SizeId != other.SizeId)
            {
                return false;
            }

            if (this.ColorId != other.ColorId)
            {
                return false;
            }

            return true;
        }
    }
}