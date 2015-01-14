using PoshBoutique.Data;
using PoshBoutique.Data.Models;
using PoshBoutique.Data.Search.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoshBoutique.Areas.Admin.Controllers
{
    public class SearchController : AdminControllerBase
    {
        public ActionResult Index(string q)
        {
            this.ViewBag.SearchTerm = q;

            if (!string.IsNullOrEmpty(q))
            {
                IEnumerable<ArticleModel> foundArticles = ArticlesIndexStore.Current.Find(q);

                this.ViewBag.Articles = foundArticles;
            }
            else
            {
                this.ViewBag.Articles = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ReIndex()
        {
            using (PoshBoutiqueData db = new PoshBoutiqueData())
            {
                ArticlesIndexStore.Current.UpsertItems(db.Articles.ToList());
            }

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Clean()
        {
            ArticlesIndexStore.Current.Clear();

            return this.RedirectToAction("Index");
        }
    }
}