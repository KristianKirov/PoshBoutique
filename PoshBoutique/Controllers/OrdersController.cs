using PoshBoutique.Models;
using PoshBoutique.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        [HttpPost]
        [Route("Validate")]
        public async Task<IHttpActionResult> Validate(ClientOrderModel order)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (order.Items.Count() == 0)
            {
                return this.BadRequest("Empty order");
            }

            List<OrderedItemModel> items = order.Items.ToList();
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
                        return this.BadRequest("Different prices of the same item");
                    }

                    totalQuantity += equalItem.Quantity.Value;
                }

                items.RemoveAll(i => i.Equals(currentItem));
                currentItem.Quantity = totalQuantity;

                normalizedItems.Add(currentItem);
            }

            OrderValidator orderValidator = new OrderValidator();
            bool isOrderValid = await orderValidator.ValidateOrder(normalizedItems);
            
            if (!isOrderValid)
            {
                return this.BadRequest("Invalid quantities");
            }

            return this.Ok();
        }
    }
}
