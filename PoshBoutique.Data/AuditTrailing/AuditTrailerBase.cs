using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.AuditTrailing
{
    public abstract class AuditTrailerBase
    {
        internal abstract void ProcessEntities(DbChangeTracker changeTracker);
    }

    public abstract class AuditTrailerBase<T> : AuditTrailerBase where T : class
    {
        public EntityState EntityStateFilter { get; protected set; }

        public AuditTrailerBase()
        {
            this.EntityStateFilter = EntityState.Added | EntityState.Modified;
        }

        protected abstract void ProcessEntities(IEnumerable<DbEntityEntry<T>> entitesToProcess);

        internal override void ProcessEntities(DbChangeTracker changeTracker)
        {
            IEnumerable<DbEntityEntry<T>> entitesToProcess = changeTracker.Entries<T>().Where(e => (this.EntityStateFilter & e.State) == e.State);

            this.ProcessEntities(entitesToProcess);
        }
    }
}
