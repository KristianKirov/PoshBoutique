using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class NewOrderEmailModel
    {
        public string FullName { get; set; }
        public int OrderNumber { get; set; }
        public string OrdersPageUrl { get; set; }
        public string ContactUsFormUrl { get; set; }
        public List<Item> Items { get; set; }
        public string DeliveryMerchant { get; set; }
        public double CommissionPercents { get; set; }
        public string ShippingPrice { get; set; }
        public string TotalPrice { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Commune { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string AddresDetails { get; set; }
        public string BaseUrl { get; set; }
    }

    public class Item
    {
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
    }
}