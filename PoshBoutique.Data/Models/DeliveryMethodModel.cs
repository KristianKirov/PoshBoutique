using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class DeliveryMethodModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public decimal DeliveryPrice { get; set; }
        public double CODTax { get; set; }
        public int OrderIndex { get; set; }
    }
}
