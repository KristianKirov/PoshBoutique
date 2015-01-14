using PoshBoutique.Polyfills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PoshBoutique.ScheduledTasks
{
    public abstract class ScheduledTaskBase<T>
    {
        public void Execute(T context)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                await this.ExecuteCore(ct, context);
            });
        }

        protected abstract Task ExecuteCore(CancellationToken cancellationToken, T context);
    }
}