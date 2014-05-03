using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PoshBoutique.Data;

namespace PoshBoutique.Areas.Admin.Controllers
{
    public class ArticleCategoriesController : Controller
    {
        private Entities db = new Entities();

        // GET: /Admin/ArticleCategories/Create
        public ActionResult Create(int? articleId)
        {
            if (articleId != null)
            {
                ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleId.Value);
            }
            else
            {
                ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Title");

            return View();
        }

        // POST: /Admin/Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int articleId, int categoryId)
        {
            if (ModelState.IsValid)
            {
                Article article = db.Articles.Find(articleId);
                Category category = db.Categories.Find(categoryId);

                Category currentCategory = category;
                while (currentCategory != null)
                {
                    if (!article.Categories.Contains(currentCategory))
                    {
                        article.Categories.Add(currentCategory);
                    }

                    currentCategory = currentCategory.ParentCategory;
                }

                db.SaveChanges();

                return RedirectToAction("Details", "Articles", new { id = article.Id });
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "Title", articleId);
            ViewBag.CategoryId = new SelectList(db.Colors, "Id", "Title", categoryId);

            return View();
        }
        // GET: /Admin/Stocks/Delete/5
        public ActionResult Delete(int articleId, int categoryId)
        {
            Article article = db.Articles.Find(articleId);
            if (article == null)
            {
                return HttpNotFound();
            }

            Category category = article.Categories.First(c => c.Id == categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.Article = article;
            ViewBag.Category = category;

            return View();
        }

        // POST: /Admin/Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int articleId, int categoryId)
        {
            Article article = db.Articles.Find(articleId);
            if (article == null)
            {
                return HttpNotFound();
            }

            Category category = article.Categories.First(c => c.Id == categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            article.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Details", "Articles", new { id = article.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
