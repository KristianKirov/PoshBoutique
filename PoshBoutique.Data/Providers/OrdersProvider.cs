using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PoshBoutique.Data.Providers
{
    public class OrdersProvider
    {
        public async Task<int> SaveOrder(Order order, IEnumerable<OrderDetail> orderDetails)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                foreach (OrderDetail orderDetail in orderDetails)
                {
                    order.OrderDetails.Add(orderDetail);
                }

                dataContext.Orders.Add(order);

                await dataContext.SaveChangesAsync();
            }

            return order.Id;
        }

        public async Task<SimpleOrderModel> GetOrder(int id)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                Order order = await dataContext.Orders.FindAsync(id);

                return new SimpleOrderModel()
                {
                    Id = order.Id,
                    PaymentMethodId = order.PaymentMethodId,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId
                };
            }
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserId(Guid userId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                IEnumerable<OrderModel> userOrders = await dataContext.Orders.Where(o => o.UserId == userId).
                    OrderByDescending(o => o.DateCreated).
                    Include(o => o.OrderStatus).
                    Select(o => new OrderModel()
                    {
                        Id = o.Id,
                        DeliveryMerchant = o.DeliveryMerchant,
                        DeliveryPrice = o.DeliveryPrice,
                        HasComission = o.HasCommission,
                        CommissionPercents = o.CommissionPercents,
                        ShippingPrice = o.ShippingPrice,
                        ItemsPrice = o.ItemsPrice,
                        TotalPrice = o.TotalPrice,
                        Status = o.OrderStatus.Name,
                        DateCreated = o.DateCreated
                    }).
                    ToArrayAsync();

                return userOrders;
            }
        }

        public async Task<IEnumerable<OrderItemModel>> GetOrderItems(int orderId, Guid userId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                IEnumerable<OrderItemModel> orderedItems = await dataContext.OrderDetails.
                    Include(od => od.Article).
                    Include(od => od.Size).
                    Include(od => od.Color).
                    Where(od => od.OrderId == orderId && od.Order.UserId == userId).
                    Select(od => new OrderItemModel()
                    {
                        ArticleTitle = od.Article.Title,
                        ArticleUrlName = od.Article.UrlName,
                        ThumbnailUrl = od.Article.ThumbnailUrl,
                        Size = od.Size.Name,
                        Color = od.ColorId == null ? null : od.Color.Title,
                        Quantity = od.Quantity,
                        ItemPrice = od.ItemPrice,
                    }).
                    OrderBy(oi => oi.ArticleTitle).
                    ToArrayAsync();

                return orderedItems;
            }
        }

        public async Task<IEnumerable<StatusHistoryModel>> GetOrderHistory(int orderId, Guid userId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                IEnumerable<StatusHistoryModel> statusHistory = await dataContext.OrderStatusesHistories.
                    Include(h => h.OrderStatus).
                    Where(h => h.OrderId == orderId && h.Order.UserId == userId).
                    OrderBy(h => h.StatusAssignDate).
                    Select(h => new StatusHistoryModel()
                    {
                        Name = h.OrderStatus.Name,
                        DateAssigned = h.StatusAssignDate
                    })
                    .ToArrayAsync();

                return statusHistory;
            }
        }

        public async Task<Order> GetFullOrderData(int orderId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                return await dataContext.Orders.Include(o => o.OrderDetails).Include(o => o.OrderDetails.Select(od => od.Article)).FirstOrDefaultAsync(o => o.Id == orderId);
            }
        }

        public async Task<OrderStatusModel> GetStatus(int statusId)
        {
            using (PoshBoutiqueData dataContext = new PoshBoutiqueData())
            {
                OrderStatus orderStatus = await dataContext.OrderStatuses.FindAsync(statusId);
                if (orderStatus == null)
                {
                    return null;
                }

                return new OrderStatusModel()
                {
                    Id = orderStatus.Id,
                    Name = orderStatus.Name
                };
            }
        }
    }
}
