using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Areas.Admin.Models
{
    public class DiscountModel
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
    }
}