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
    public class basketsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: baskets
        public ActionResult Index()
        {
            var baskets = db.baskets.Include(b => b.client);
            return View(baskets.ToList());
        }

        // GET: baskets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basket basket = db.baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // GET: baskets/Create
        public ActionResult Create()
        {
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname");
            return View();
        }

        // POST: baskets/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,client_id")] basket basket)
        {
            if (ModelState.IsValid)
            {
                db.baskets.Add(basket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", basket.client_id);
            return View(basket);
        }

        // GET: baskets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basket basket = db.baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", basket.client_id);
            return View(basket);
        }

        // POST: baskets/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,client_id")] basket basket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(basket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", basket.client_id);
            return View(basket);
        }

        // GET: baskets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            basket basket = db.baskets.Find(id);
            if (basket == null)
            {
                return HttpNotFound();
            }
            return View(basket);
        }

        // POST: baskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            basket basket = db.baskets.Find(id);
            db.baskets.Remove(basket);
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
