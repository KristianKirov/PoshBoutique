using PoshBoutique.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoshBoutique.Areas.Admin.Controllers
{
    public class DiscountsController : Controller
    {
        [HttpGet]
        public ActionResult Apply(int articleId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Apply(int articleId, DiscountModel discount)
        {
            return View();
        }

        //
        // GET: /Admin/Discounts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Discounts/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Discounts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Discounts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Discounts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Discounts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Discounts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
