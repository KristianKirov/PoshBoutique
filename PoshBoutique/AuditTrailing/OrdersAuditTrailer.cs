using PoshBoutique.Data;
using PoshBoutique.Data.AuditTrailing;
using PoshBoutique.Data.Models;
using PoshBoutique.Facades;
using PoshBoutique.Identity.Providers;
using PoshBoutique.ScheduledTasks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PoshBoutique.AuditTrailing
{
    public class OrdersAuditTrailer : AuditTrailerBase<Order>
    {
        protected override void ProcessEntities(IEnumerable<DbEntityEntry<Order>> entitesToProcess)
        {
            foreach (DbEntityEntry<Order> orderEntry in entitesToProcess)
            {
                DbPropertyEntry statusIdProperty = orderEntry.Property("StatusId");
                bool isStatusChanged = orderEntry.State != EntityState.Added && statusIdProperty.IsModified && ((int)statusIdProperty.OriginalValue) != ((int)statusIdProperty.CurrentValue);
                if (orderEntry.State == EntityState.Added || isStatusChanged)
                {
                    Order order = orderEntry.Entity;

                    order.OrderStatusesHistory.Add(new OrderStatusesHistory()
                    {
                        StatusId = order.StatusId,
                        StatusAssignDate = DateTime.UtcNow
                    });
                }

                if (isStatusChanged)
                {
                    Order order = orderEntry.Entity;
                    int oldStatusId = (int)statusIdProperty.OriginalValue;
                    int newStatusId = (int)statusIdProperty.CurrentValue;

                    MailSendingFacade mailSender = new MailSendingFacade();
                    mailSender.SendOrderStatusChangedMail(order.Id, oldStatusId, newStatusId);

                    if (newStatusId == 4) //Изпълнена
                    {
                        RegisterExpenseTask registerExpenseTask = new RegisterExpenseTask();
                        registerExpenseTask.Execute(new RegisterExpenseContext()
                        {
                            OrderId = order.Id,
                            UserId = order.UserId,
                            ExpenseAmount = order.TotalPrice
                        });
                    }
                }

                if (orderEntry.State == EntityState.Added)
                {
                    IEnumerable<ArticleOrderCountModel> articleOrderCounts = orderEntry.Entity.OrderDetails.Select(o =>
                        new ArticleOrderCountModel()
                        {
                            ArticleId = o.ItemId,
                            OrderCount = o.Quantity
                        }).ToArray();

                    UpdateArticlesOrdersCountsTask updateArticlesOrdersCountsTask = new UpdateArticlesOrdersCountsTask();
                    updateArticlesOrdersCountsTask.Execute(articleOrderCounts);
                }
            }
        }
    }
}