using Microsoft.AspNet.Identity;
using PoshBoutique.Facades;
using PoshBoutique.Identity;
using PoshBoutique.Identity.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.ScheduledTasks
{
    public class RegisterExpenseTask : ScheduledTaskBase<RegisterExpenseContext>
    {
        protected override async Task ExecuteCore(CancellationToken cancellationToken, RegisterExpenseContext context)
        {
            ExpensesProvider expensesProvider = new ExpensesProvider();
            decimal newTotalAmount = await expensesProvider.RegisterUserExpense(context.UserId, context.ExpenseAmount);

            if (newTotalAmount >= 1000)
            {
                IdentityResult result;
                using (ApplicationUserManager userManager = Startup.UserManagerFactory())
                {
                    result = await userManager.AddToRoleAsync(context.UserId.ToString(), "LoyalCustomer");
                }

                if (result.Succeeded)
                {
                    MailSendingFacade mailSender = new MailSendingFacade();
                    await mailSender.SendLoyalCustomerMail(context.UserId, context.OrderId);
                }
            }
        }
    }
}