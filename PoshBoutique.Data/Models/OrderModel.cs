using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string DeliveryMerchant { get; set; }

        public decimal DeliveryPrice { get; set; }

        public bool HasComission { get; set; }

        public double CommissionPercents { get; set; }

        public decimal ShippingPrice { get; set; }

        public decimal ItemsPrice { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
