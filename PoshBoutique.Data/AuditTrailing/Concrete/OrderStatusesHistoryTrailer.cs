using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.AuditTrailing.Concrete
{
    public class OrderStatusesHistoryTrailer : AuditTrailerBase<Order>
    {
        protected override void ProcessEntities(IEnumerable<DbEntityEntry<Order>> entitesToProcess)
        {
            foreach (DbEntityEntry<Order> orderEntry in entitesToProcess)
            {
                DbPropertyEntry statusIdProperty = orderEntry.Property("StatusId");
                if (orderEntry.State == EntityState.Added || statusIdProperty.IsModified)
                {
                    Order order = orderEntry.Entity;

                    order.OrderStatusesHistory.Add(new OrderStatusesHistory()
                    {
                        StatusId = order.StatusId,
                        StatusAssignDate = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
