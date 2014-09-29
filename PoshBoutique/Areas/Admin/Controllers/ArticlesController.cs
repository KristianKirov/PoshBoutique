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
    public class ArticlesController : Controller
    {
        private PoshBoutiqueData db = new PoshBoutiqueData();

        // GET: /Admin/Articles/
        public ActionResult Index()
        {
            var articles = db.Articles.Include(a => a.SizeType).Include(a => a.Categories);
            return View(articles.ToList());
        }

        // GET: /Admin/Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = article.Categories.ToList();
            return View(article);
        }

        // GET: /Admin/Articles/Create
        public ActionResult Create()
        {
            ViewBag.SizeTypeId = new SelectList(db.SizeTypes, "Id", "Title");
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name");

            return View();
        }

        // POST: /Admin/Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UrlName,Title,DateCreated,Price,Description,ShortDescription,MaterialDescription,ThumbnailUrl,SizeTypeId,Visible,LikesCount,OrdersCount,CollectionId,IsFeatured")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SizeTypeId = new SelectList(db.SizeTypes, "Id", "Title", article.SizeTypeId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", article.CollectionId);

            return View(article);
        }

        // GET: /Admin/Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.SizeTypeId = new SelectList(db.SizeTypes, "Id", "Title", article.SizeTypeId);
            ViewBag.Categories = article.Categories.ToList();
            ViewBag.RelatedArticles = article.RelatedArticles.OrderByDescending(a => a.DateCreated).ToList();
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", article.CollectionId);

            return View(article);
        }

        // POST: /Admin/Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UrlName,Title,DateCreated,Price,Description,ShortDescription,MaterialDescription,ThumbnailUrl,SizeTypeId,Visible,OriginalPrice,DiscountDescription,LikesCount,OrdersCount,CollectionId,IsFeatured")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SizeTypeId = new SelectList(db.SizeTypes, "Id", "Title", article.SizeTypeId);
            ViewBag.Categories = article.Categories.ToList();
            ViewBag.RelatedArticles = article.RelatedArticles.OrderByDescending(a => a.DateCreated).ToList();
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", article.CollectionId);

            return View(article);
        }

        public ActionResult DeleteRelatedArticle(int articleId, int relatedArticleId)
        {
            Article article = db.Articles.Find(articleId);
            Article relatedArticle = article.RelatedArticles.First(a => a.Id == relatedArticleId);
            article.RelatedArticles.Remove(relatedArticle);

            db.SaveChanges();

            return RedirectToAction("Edit", new { id = articleId });
        }

        [HttpGet]
        public ActionResult AddRelatedArticle(int articleId)
        {
            Article article = db.Articles.Find(articleId);
            ViewBag.RelatedArticleId = new SelectList(db.Articles, "Id", "Title");

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRelatedArticle(int articleId, int relatedArticleId)
        {
            Article article = db.Articles.Find(articleId);
            Article relatedArticle = db.Articles.Find(relatedArticleId);
            if (!article.RelatedArticles.Contains(relatedArticle))
            {
                article.RelatedArticles.Add(relatedArticle);

                db.SaveChanges();
            }

            return RedirectToAction("Edit", new { id = articleId });
        }

        // GET: /Admin/Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: /Admin/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
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
