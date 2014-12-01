using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public class SimpleOrderModel
    {
        public int Id { get; set; }

        public int PaymentMethodId { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
