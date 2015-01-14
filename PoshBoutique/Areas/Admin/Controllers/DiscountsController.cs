using PoshBoutique.Areas.Admin.Models;
using PoshBoutique.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PoshBoutique.Areas.Admin.Controllers
{
    public class DiscountsController : AdminControllerBase
    {
        private PoshBoutiqueData db = new PoshBoutiqueData();

        [HttpGet]
        public ActionResult Apply(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            ViewBag.ArticleTitle = article.Title;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(int id, DiscountModel discount)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            if (article.OriginalPrice != null)
            {
                article.Price = article.OriginalPrice.Value;
            }

            article.OriginalPrice = article.Price;
            article.DiscountDescription = discount.Description;
            if (discount.Type == 1)
            {
                article.Price = (article.Price * (100 - discount.Value)) / 100;
            }
            else if (discount.Type == 2)
            {
                article.Price = article.Price - discount.Value;
            }

            article.DiscountDescription = discount.Description;

            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            if (article.OriginalPrice == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ArticleTitle = article.Title;

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            article.Price = article.OriginalPrice.Value;
            article.OriginalPrice = null;
            article.DiscountDescription = null;

            db.SaveChanges();

            return RedirectToAction("Index", "Articles");
        }
    }
}
