using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class OrderItemsGrouper
    {
        public IList<OrderedItemModel> NormalizeOrderedItems(List<OrderedItemModel> items)
        {
            List<OrderedItemModel> normalizedItems = new List<OrderedItemModel>();
            while (items.Count > 0)
            {
                OrderedItemModel currentItem = items[0];
                OrderedItemModel[] equalItems = items.Where(i => i.Equals(currentItem)).ToArray();
                int totalQuantity = 0;
                foreach (OrderedItemModel equalItem in equalItems)
                {
                    if (equalItem.Price != currentItem.Price)
                    {
                        return null;
                    }

                    totalQuantity += equalItem.Quantity.Value;
                }

                items.RemoveAll(i => i.Equals(currentItem));
                currentItem.Quantity = totalQuantity;

                normalizedItems.Add(currentItem);
            }

            return normalizedItems;
        }
    }

    public class ClientOrderModel
    {
        [Required]
        public IEnumerable<OrderedItemModel> Items { get; set; }
    }

    public class FullOrderModel : ClientOrderModel
    {
        [Required]
        public int? PaymentMethodId { get; set; }

        [Required]
        public int? DeliveryMethodId { get; set; }

        [Required]
        public OrderTotal Total { get; set; }
    }

    public class OrderTotal
    {
        [Required]
        public decimal? Shipping { get; set; }

        [Required]
        public decimal? Order { get; set; }

        [Required]
        public decimal? Full { get; set; }
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