using PoshBoutique.AuditTrailing;
using PoshBoutique.Data;
using PoshBoutique.Data.AuditTrailing.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique
{
    public static class AuditTrailingRegistration
    {
        public static void RegisterAllAuditTrailers()
        {
            PoshBoutiqueData.RegisterAuditTrailer(new OrderStatusesHistoryTrailer());
            PoshBoutiqueData.RegisterAuditTrailer(new ArticleSearchIndexTrailer());
        }
    }
}