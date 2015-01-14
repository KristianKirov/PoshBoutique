using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Models
{
    public class OrderStatusChangedEmailModel
    {
        public string FullName { get; set; }

        public int OrderNumber { get; set; }

        public string OldOrderStatus { get; set; }

        public string NewOrderStatus { get; set; }

        public string BaseUrl { get; set; }
    }
}