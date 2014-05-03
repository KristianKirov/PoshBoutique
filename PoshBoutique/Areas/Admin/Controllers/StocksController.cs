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
    public class StocksController : Controller
    {
        private Entities db = new Entities();

        // GET: /Admin/Stocks/
        public ActionResult Index()
        {
            var stocks = db.Stocks.Include(s => s.Article).Include(s => s.Color).Include(s => s.Size);
            return View(stocks.ToList());
        }

        // GET: /Admin/Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: /Admin/Stocks/Create
        public ActionResult Create()
        {
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName");
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Title");
            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name");
            return View();
        }

        // POST: /Admin/Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ArticleId,SizeId,ColorId,Quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", stock.ArticleId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Title", stock.ColorId);
            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name", stock.SizeId);
            return View(stock);
        }

        // GET: /Admin/Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", stock.ArticleId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Title", stock.ColorId);
            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name", stock.SizeId);
            return View(stock);
        }

        // POST: /Admin/Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ArticleId,SizeId,ColorId,Quantity")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleId = new SelectList(db.Articles, "Id", "UrlName", stock.ArticleId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "Title", stock.ColorId);
            ViewBag.SizeId = new SelectList(db.Sizes, "Id", "Name", stock.SizeId);
            return View(stock);
        }

        // GET: /Admin/Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: /Admin/Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
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
