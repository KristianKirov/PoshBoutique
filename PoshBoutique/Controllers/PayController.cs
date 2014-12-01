using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PoshBoutique.Controllers
{
    public class PayController : Controller
    {
        public async Task<ActionResult> Order(int id)
        {
            OrdersProvider ordersProvider = new OrdersProvider();
            SimpleOrderModel order = await ordersProvider.GetOrder(id);

            if (order.PaymentMethodId == 1)
            {
                return this.View("Epay", order);
            }

            return this.Redirect("/order-accepted");
        }
	}
}