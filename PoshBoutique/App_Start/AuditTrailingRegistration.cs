using PoshBoutique.AuditTrailing;
using PoshBoutique.Data;
using PoshBoutique.Facades;
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
            PoshBoutiqueData.RegisterAuditTrailer(new OrdersAuditTrailer());
            PoshBoutiqueData.RegisterAuditTrailer(new ArticleSearchIndexTrailer());
        }
    }
}