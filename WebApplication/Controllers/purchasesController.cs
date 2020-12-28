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
    public class purchasesController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: purchases
        public ActionResult Index()
        {
            var purchases = db.purchases.Include(p => p.client).Include(p => p.product);
            return View(purchases.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string search)
        {
            var result = db.purchases
              .Where(a => a.productname.ToLower().Contains(search.ToLower()) || a.client.login.ToLower().Contains(search.ToLower()) || a.date.ToString().Contains(search))
              .ToList();

            return View(result);
        }

        // GET: purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: purchases/Create
        public ActionResult Create()
        {
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname");
            ViewBag.product_id = new SelectList(db.products, "id", "name");
            return View();
        }

        // POST: purchases/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,client_id,product_id,count")] purchase purchase)
        {
            if (ModelState.IsValid)
            {
                purchase.date = DateTime.Now;
                product p = db.products.Find(purchase.product_id);
                purchase.productname = p.name;
                purchase.productprice = p.price;
                db.purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", purchase.client_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", purchase.product_id);
            return View(purchase);
        }

        // GET: purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", purchase.client_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", purchase.product_id);
            return View(purchase);
        }

        // POST: purchases/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,client_id,product_id,productname,productprice,count,date")] purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", purchase.client_id);
            ViewBag.product_id = new SelectList(db.products, "id", "name", purchase.product_id);
            return View(purchase);
        }

        // GET: purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchase purchase = db.purchases.Find(id);
            db.purchases.Remove(purchase);
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
