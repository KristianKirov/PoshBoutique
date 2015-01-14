using PoshBoutique.Data.Models;
using PoshBoutique.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.ScheduledTasks
{
    public class UpdateArticlesOrdersCountsTask : ScheduledTaskBase<IEnumerable<ArticleOrderCountModel>>
    {
        protected override async Task ExecuteCore(CancellationToken cancellationToken, IEnumerable<ArticleOrderCountModel> context)
        {
            ArticlesProvider articlesProvider = new ArticlesProvider();
            await articlesProvider.UpdateOrdersCounts(context);
        }
    }
}