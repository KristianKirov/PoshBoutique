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
    public class SizeTypesController : Controller
    {
        private PoshBoutiqueData db = new PoshBoutiqueData();

        // GET: /Admin/SizeTypes/
        public ActionResult Index()
        {
            return View(db.SizeTypes.ToList());
        }

        // GET: /Admin/SizeTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeType sizetype = db.SizeTypes.Find(id);
            if (sizetype == null)
            {
                return HttpNotFound();
            }
            return View(sizetype);
        }

        // GET: /Admin/SizeTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/SizeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Title")] SizeType sizetype)
        {
            if (ModelState.IsValid)
            {
                db.SizeTypes.Add(sizetype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sizetype);
        }

        // GET: /Admin/SizeTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeType sizetype = db.SizeTypes.Find(id);
            if (sizetype == null)
            {
                return HttpNotFound();
            }
            return View(sizetype);
        }

        // POST: /Admin/SizeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Title")] SizeType sizetype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sizetype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sizetype);
        }

        // GET: /Admin/SizeTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeType sizetype = db.SizeTypes.Find(id);
            if (sizetype == null)
            {
                return HttpNotFound();
            }
            return View(sizetype);
        }

        // POST: /Admin/SizeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SizeType sizetype = db.SizeTypes.Find(id);
            db.SizeTypes.Remove(sizetype);
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
