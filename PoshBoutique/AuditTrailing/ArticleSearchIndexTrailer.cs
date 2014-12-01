using PoshBoutique.Data;
using PoshBoutique.Data.AuditTrailing;
using PoshBoutique.Data.Search.Articles;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PoshBoutique.AuditTrailing
{
    public class ArticleSearchIndexTrailer : AuditTrailerBase<Article>
    {
        protected override void ProcessEntities(IEnumerable<DbEntityEntry<Article>> entitesToProcess)
        {
            ArticlesIndexStore.Current.UpsertItems(entitesToProcess.Select(e => e.Entity).ToArray());
        }
    }
}