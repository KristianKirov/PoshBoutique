using PoshBoutique.Models;
using PoshBoutique.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PoshBoutique.Identity;
using System.Data.Entity;
using PoshBoutique.Data.Providers;
using PoshBoutique.Data.Models;
using PoshBoutique.Data;

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

            OrderItemsGrouper itemsGrouper = new OrderItemsGrouper();
            IList<OrderedItemModel> normalizedItems = itemsGrouper.NormalizeOrderedItems(order.Items.ToList());
            if (normalizedItems == null)
            {
                return this.BadRequest("Different prices of the same item");
            }

            OrderValidator orderValidator = new OrderValidator();
            bool isOrderValid = await orderValidator.ValidateOrderItems(normalizedItems);
            
            if (!isOrderValid)
            {
                return this.BadRequest("Invalid quantities");
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("GetAddressInfo")]
        [Authorize]
        public async Task<IHttpActionResult> GetAddressInfo()
        {
            AddressInfoModel addressInfo = new AddressInfoModel();
            string userId = this.User.Identity.GetUserId();
            using (ApplicationDbContext usersDbContext = new ApplicationDbContext())
            {
                Address userAddress = await usersDbContext.AddressInfos.FirstOrDefaultAsync(a => a.UserId == userId);
                if (userAddress != null)
                {
                    addressInfo.FirstName = userAddress.FirstName;
                    addressInfo.MiddleName = userAddress.MiddleName;
                    addressInfo.LastName = userAddress.LastName;
                    addressInfo.Email = userAddress.Email;
                    addressInfo.Phone = userAddress.Phone;
                    addressInfo.Country = userAddress.Country;
                    addressInfo.District = userAddress.District;
                    addressInfo.Commune = userAddress.Commune;
                    addressInfo.City = userAddress.City;
                    addressInfo.PostCode = userAddress.PostCode;
                    addressInfo.Address = userAddress.AddresDetails;
                }
                else
                {
                    ApplicationUser currentUser = usersDbContext.Users.First(u => u.Id == userId);
                    addressInfo.FirstName = currentUser.FirstName;
                    addressInfo.LastName = currentUser.LastName;
                    addressInfo.Email = currentUser.Email;
                    addressInfo.Country = "България"; //TODO: add countries nomenclature
                }
            }

            return this.Ok(addressInfo);
        }

        [HttpPost]
        [Route("SetAddressInfo")]
        [Authorize]
        public async Task<IHttpActionResult> SetAddressInfo([FromBody]AddressInfoModel addressInfo)
        {
            using (ApplicationDbContext usersDbContext = new ApplicationDbContext())
            {
                string userId = this.User.Identity.GetUserId();
                Address userAddress = await usersDbContext.AddressInfos.FirstOrDefaultAsync(a => a.UserId == userId);
                if (userAddress == null)
                {
                    userAddress = new Address();
                    userAddress.UserId = userId;
                    usersDbContext.AddressInfos.Add(userAddress);
                }

                userAddress.FirstName = addressInfo.FirstName;
                userAddress.MiddleName = addressInfo.MiddleName;
                userAddress.LastName = addressInfo.LastName;
                userAddress.Email = addressInfo.Email;
                userAddress.Phone = addressInfo.Phone;
                userAddress.Country = addressInfo.Country;
                userAddress.District = addressInfo.District;
                userAddress.Commune = addressInfo.Commune;
                userAddress.City = addressInfo.City;
                userAddress.PostCode = addressInfo.PostCode;
                userAddress.AddresDetails = addressInfo.Address;

                await usersDbContext.SaveChangesAsync();
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("DeliveryMethods")]
        [Authorize]
        public async Task<IHttpActionResult> GetDeliveryMethods()
        {
            DeliveryMethodsProvider deliveryMethodsProvider = new DeliveryMethodsProvider();
            IEnumerable<DeliveryMethodModel> allDeliveryMethods = await deliveryMethodsProvider.GetAllDeliveryMethods();

            return this.Ok(allDeliveryMethods);
        }

        [HttpGet]
        [Route("PaymentMethods")]
        [Authorize]
        public async Task<IHttpActionResult> GetPaymentMethods()
        {
            PaymentMethodsProvider paymentMethodsProvider = new PaymentMethodsProvider();
            IEnumerable<PaymentMethodModel> allPaymentMethods = await paymentMethodsProvider.GetAllPaymentMethods();

            return this.Ok(allPaymentMethods);
        }

        [HttpPost]
        [Route("ValidateAndSaveOrder")]
        [Authorize]
        public async Task<IHttpActionResult> ValidateAndSaveOrder(FullOrderModel order)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (order.Items.Count() == 0)
            {
                return this.BadRequest("Empty order");
            }

            OrderItemsGrouper itemsGrouper = new OrderItemsGrouper();
            IList<OrderedItemModel> normalizedItems = itemsGrouper.NormalizeOrderedItems(order.Items.ToList());
            if (normalizedItems == null)
            {
                return this.BadRequest("Different prices of the same item");
            }

            OrderValidator orderValidator = new OrderValidator();
            bool areOrderItemsValid = await orderValidator.ValidateOrderItems(normalizedItems);

            if (!areOrderItemsValid)
            {
                return this.BadRequest("Invalid quantities");
            }

            bool areOrderPricesValid = await orderValidator.ValidatePrices(order);
            if (!areOrderPricesValid)
            {
                return this.BadRequest("Invalid price");
            }

            DeliveryMethodsProvider deliveryMethodsProvider = new DeliveryMethodsProvider();
            Task<DeliveryMethodModel> getDeliveryMethodTask = deliveryMethodsProvider.GetDeliveryMethodById(order.DeliveryMethodId.Value);

            PaymentMethodsProvider paymentMethodsProvider = new PaymentMethodsProvider();
            Task<PaymentMethodModel> getPaymentMethodTask = paymentMethodsProvider.GetPaymentMethodById(order.PaymentMethodId.Value);

            DeliveryMethodModel deliveryMethod = await getDeliveryMethodTask;
            PaymentMethodModel paymentMethod = await getPaymentMethodTask;

            Order newOrder = new Order();
            newOrder.UserId = new Guid(this.User.Identity.GetUserId());
            newOrder.DeliveryMerchant = deliveryMethod.Name;
            newOrder.DeliveryPrice = deliveryMethod.DeliveryPrice;
            newOrder.PaymentMethodId = paymentMethod.Id;
            newOrder.HasCommission = paymentMethod.ApplyDeliveryTax;
            newOrder.CommissionPercents = deliveryMethod.CODTax;
            newOrder.ShippingPrice = order.Total.Shipping.Value;
            newOrder.ItemsPrice = order.Total.Order.Value;
            newOrder.TotalPrice = order.Total.Full.Value;
            newOrder.StatusId = 1;
            newOrder.DateCreated = DateTime.UtcNow;

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (OrderedItemModel orderedItem in order.Items)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.ItemId = orderedItem.ArticleId.Value;
                orderDetail.SizeId = orderedItem.SizeId.Value;
                orderDetail.ColorId = orderedItem.ColorId;
                orderDetail.Quantity = orderedItem.Quantity.Value;
                orderDetail.ItemPrice = orderedItem.Price.Value;

                orderDetails.Add(orderDetail);
            }

            OrdersProvider ordersProvider = new OrdersProvider();
            int newOrderId = await ordersProvider.SaveOrder(newOrder, orderDetails);

            return this.Ok(newOrderId);
        }

        [HttpGet]
        [Route("My")]
        [Authorize]
        public async Task<IHttpActionResult> GetCurrentUserOrders()
        {
            Guid userId = new Guid(this.User.Identity.GetUserId());

            OrdersProvider ordersProvider = new OrdersProvider();
            IEnumerable<OrderModel> userOrders = await ordersProvider.GetOrdersByUserId(userId);

            return this.Ok(userOrders);
        }

        [HttpGet]
        [Route("{orderId}/items")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrderItems(int orderId)
        {
            Guid userId = new Guid(this.User.Identity.GetUserId());

            OrdersProvider ordersProvider = new OrdersProvider();
            IEnumerable<OrderItemModel> orderItems = await ordersProvider.GetOrderItems(orderId, userId);

            return this.Ok(orderItems);
        }

        [HttpGet]
        [Route("{orderId}/history")]
        [Authorize]
        public async Task<IHttpActionResult> GetOrderHistory(int orderId)
        {
            Guid userId = new Guid(this.User.Identity.GetUserId());

            OrdersProvider ordersProvider = new OrdersProvider();
            IEnumerable<StatusHistoryModel> orderItems = await ordersProvider.GetOrderHistory(orderId, userId);

            return this.Ok(orderItems);
        }
    }
}
