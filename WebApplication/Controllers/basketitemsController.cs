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
    public class basketitemsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: basketitems
        public ActionResult Index()
        {
            var basketitems = db.basketitems.Include(b => b.basket).Include(b => b.product);
            return View(basketitems.ToList());
        }

        // GET: basketitems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basketitem basketitem = db.basketitems.Find(id);
            if (basketitem == null)
            {
                return HttpNotFound();
            }
            return View(basketitem);
        }

        // GET: basketitems/Create
        public ActionResult Create()
        {
            ViewBag.basket_id = new SelectList(db.baskets, "id", "id");
            ViewBag.product_id = new SelectList(db.products, "id", "name");
            return View();
        }

        // POST: basketitems/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,basket_id,product_id,count")] basketitem basketitem)
        {
            if (ModelState.IsValid)
            {
                db.basketitems.Add(basketitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.basket_id = new SelectList(db.baskets, "id", "id", basketitem.basket_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", basketitem.product_id);
            return View(basketitem);
        }

        // GET: basketitems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basketitem basketitem = db.basketitems.Find(id);
            if (basketitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.basket_id = new SelectList(db.baskets, "id", "id", basketitem.basket_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", basketitem.product_id);
            return View(basketitem);
        }

        // POST: basketitems/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,basket_id,product_id,count")] basketitem basketitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basketitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.basket_id = new SelectList(db.baskets, "id", "id", basketitem.basket_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", basketitem.product_id);
            return View(basketitem);
        }

        // GET: basketitems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basketitem basketitem = db.basketitems.Find(id);
            if (basketitem == null)
            {
                return HttpNotFound();
            }
            return View(basketitem);
        }

        // POST: basketitems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            basketitem basketitem = db.basketitems.Find(id);
            db.basketitems.Remove(basketitem);
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
