using Common.Logging.EnterpriseLibrary;
using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using PoshBoutique.Identity;
using PoshBoutique.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PoshBoutique.Controllers
{
    [RoutePrefix("api/Test")]
    public class TestController : ApiController
    {
        public IHttpActionResult Public()
        {
            return this.Ok();
        }

        [HttpPost]
        public IHttpActionResult Auth([FromBody]string userName, string password)
        {
            if (userName == "Gancho" && password == "Gankin")
            {
                return this.Json<object>(new { token = "123" });
            }

            return this.StatusCode(HttpStatusCode.Unauthorized);
        }

        [HttpGet]
        public IHttpActionResult Private()
        {
            if (this.Request.Headers.Authorization == null || this.Request.Headers.Authorization.Parameter != "123")
            {
                return this.StatusCode(HttpStatusCode.Unauthorized);
            }

            return this.Json<object>(new { Address = "Sofia" });
        }

        [HttpGet]
        [Route("UpdateQties")]
        public async Task<IHttpActionResult> UpdateQties()
        {
            StocksProvider stocksProvider = new StocksProvider();
            bool stockQuantitiesUpdatedSuccessfully = await stocksProvider.UpdateStocks(
                new []
                {
                    new StockChangeModel(3, 1, 1, -1),
                    new StockChangeModel(3, 1, 2, -1),
                    new StockChangeModel(3, 1, 0, -2),
                },
                Logger.Current);

            return this.Ok(stockQuantitiesUpdatedSuccessfully);
        }

        [HttpGet]
        [Route("Go")]
        public async Task<IHttpActionResult> Go()
        {
            decimal newVal = -1;

            using (ApplicationDbContext userManager = new ApplicationDbContext())
            {
                SqlParameter userIdParam = new SqlParameter("userId", "c9376d7f-035b-48b5-8fc8-94d310c52842")
                {
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 128
                };

                SqlParameter expenseParam = new SqlParameter("expense", 10m)
                {
                    SqlDbType = SqlDbType.Money
                };

                SqlParameter newTotalExpensesParam = new SqlParameter("newTotalExpenses", SqlDbType.Money)
                {
                    Direction = ParameterDirection.Output
                };

                await userManager.Database.ExecuteSqlCommandAsync("sp_update_expenses @userId, @expense, @newTotalExpenses OUT",
                    userIdParam, expenseParam, newTotalExpensesParam);

                newVal = (decimal)newTotalExpensesParam.Value;
            }

            return this.Ok(newVal);
        }

        [HttpGet]
        [Route("Log")]
        public IHttpActionResult Log(string message)
        {
            Logger.Current.LogError(message);

            return this.Ok();
        }
    }
}