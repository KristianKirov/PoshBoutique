using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.Identity.Providers
{
    public class ExpensesProvider
    {
        public async Task<decimal> RegisterUserExpense(Guid userId, decimal expense)
        {
            decimal currentTotalExpenses = 0m;

            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
                SqlParameter userIdParam = new SqlParameter("userId", userId.ToString())
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 128
                };

                SqlParameter expenseParam = new SqlParameter("expense", expense)
                {
                    SqlDbType = SqlDbType.Money
                };

                SqlParameter newTotalExpensesParam = new SqlParameter("newTotalExpenses", SqlDbType.Money)
                {
                    Direction = ParameterDirection.Output
                };

                await userManager.Database.ExecuteSqlCommandAsync("sp_update_expenses @userId, @expense, @newTotalExpenses OUT",
                    userIdParam, expenseParam, newTotalExpensesParam);

                currentTotalExpenses = (decimal)newTotalExpensesParam.Value;
            }

            return currentTotalExpenses;
        }
    }
}