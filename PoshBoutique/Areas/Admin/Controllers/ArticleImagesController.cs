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
    public class ArticleImagesController : Controller
    {
        private PoshBoutiqueData db = new PoshBoutiqueData();

        // GET: /Admin/ArticleImages/
        public ActionResult Index()
        {
            var articleimages = db.ArticleImages.Include(a => a.Article);
            return View(articleimages.ToList());
        }

        // GET: /Admin/ArticleImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleImage articleimage = db.ArticleImages.Find(id);
            if (articleimage == null)
            {
                return HttpNotFound();
            }
            return View(articleimage);
        }

        // GET: /Admin/ArticleImages/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName");
            return View();
        }

        // POST: /Admin/ArticleImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ArticleId,SmallImageUrl,MediumImageUrl,LargelImageUrl,OrderIndex,Id")] ArticleImage articleimage)
        {
            if (ModelState.IsValid)
            {
                db.ArticleImages.Add(articleimage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", articleimage.ArticleId);
            return View(articleimage);
        }

        // GET: /Admin/ArticleImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleImage articleimage = db.ArticleImages.Find(id);
            if (articleimage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", articleimage.ArticleId);
            return View(articleimage);
        }

        // POST: /Admin/ArticleImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ArticleId,SmallImageUrl,MediumImageUrl,LargelImageUrl,OrderIndex,Id")] ArticleImage articleimage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleimage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", articleimage.ArticleId);
            return View(articleimage);
        }

        // GET: /Admin/ArticleImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleImage articleimage = db.ArticleImages.Find(id);
            if (articleimage == null)
            {
                return HttpNotFound();
            }
            return View(articleimage);
        }

        // POST: /Admin/ArticleImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArticleImage articleimage = db.ArticleImages.Find(id);
            db.ArticleImages.Remove(articleimage);
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
