using PoshBoutique.Data.AuditTrailing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data
{
    public partial class PoshBoutiqueData
    {
        private static readonly List<AuditTrailerBase> auditTrailers;

        static PoshBoutiqueData()
        {
            PoshBoutiqueData.auditTrailers = new List<AuditTrailerBase>();
        }

        public static void RegisterAuditTrailer(AuditTrailerBase auditTrailer)
        {
            PoshBoutiqueData.auditTrailers.Add(auditTrailer);
        }

        public override int SaveChanges()
        {
            this.ApplyAuditTrailing();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            this.ApplyAuditTrailing();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditTrailing()
        {
            foreach (AuditTrailerBase auditTrailer in PoshBoutiqueData.auditTrailers)
            {
                auditTrailer.ProcessEntities(this.ChangeTracker);
            }
        }
    }
}
