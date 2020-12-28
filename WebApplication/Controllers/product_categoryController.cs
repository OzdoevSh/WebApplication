using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class product_categoryController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: product_category
        public ActionResult Index()
        {
            var product_category = db.product_category.Include(p => p.category).Include(p => p.product);
            return View(product_category.ToList());
        }

        // GET: product_category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_category product_category = db.product_category.Find(id);
            if (product_category == null)
            {
                return HttpNotFound();
            }
            return View(product_category);
        }

        // GET: product_category/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "id", "name");
            ViewBag.product_id = new SelectList(db.products, "id", "name");
            return View();
        }

        // POST: product_category/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,product_id,category_id")] product_category product_category)
        {
            if (ModelState.IsValid)
            {
                db.product_category.Add(product_category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.categories, "id", "name", product_category.category_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", product_category.product_id);
            return View(product_category);
        }

        // GET: product_category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_category product_category = db.product_category.Find(id);
            if (product_category == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "id", "name", product_category.category_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", product_category.product_id);
            return View(product_category);
        }

        // POST: product_category/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,product_id,category_id")] product_category product_category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.categories, "id", "name", product_category.category_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", product_category.product_id);
            return View(product_category);
        }

        // GET: product_category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product_category product_category = db.product_category.Find(id);
            if (product_category == null)
            {
                return HttpNotFound();
            }
            return View(product_category);
        }

        // POST: product_category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            product_category product_category = db.product_category.Find(id);
            db.product_category.Remove(product_category);
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
