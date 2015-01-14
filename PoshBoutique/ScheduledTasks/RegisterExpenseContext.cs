using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.ScheduledTasks
{
    public class RegisterExpenseContext
    {
        public int OrderId { get; set; }

        public Guid UserId { get; set; }

        public decimal ExpenseAmount { get; set; }
    }
}