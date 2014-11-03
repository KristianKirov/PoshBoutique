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

                try
                {
                    await usersDbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }

            return this.Ok();
        }

        [HttpGet]
        [Route("DeliveryMethods")]
        [Authorize]
        public async Task<IHttpActionResult> GetDeliveryMethods([FromBody]AddressInfoModel addressInfo)
        {
            DeliveryMethodsProvider deliveryMethodsProvider = new DeliveryMethodsProvider();
            IEnumerable<DeliveryMethodModel> allDeliveryMethods = await deliveryMethodsProvider.GetAllDeliveryMethods();

            return this.Ok(allDeliveryMethods);
        }
    }
}
