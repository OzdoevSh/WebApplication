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
    public class depositsController : Controller
    {
        private shopEntities db = new shopEntities();

        // GET: deposits
        public ActionResult Index()
        {
            var deposits = db.deposits.Include(d => d.client);
            return View(deposits.ToList());
        }

        // GET: deposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deposit deposit = db.deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }

        // GET: deposits/Create
        public ActionResult Create()
        {
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname");
            return View();
        }

        // POST: deposits/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,client_id,value,version")] deposit deposit)
        {
            if (ModelState.IsValid)
            {
                db.deposits.Add(deposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", deposit.client_id);
            return View(deposit);
        }

        // GET: deposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deposit deposit = db.deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", deposit.client_id);
            return View(deposit);
        }

        // POST: deposits/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,client_id,value,version")] deposit deposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "firstname", deposit.client_id);
            return View(deposit);
        }

        // GET: deposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            deposit deposit = db.deposits.Find(id);
            if (deposit == null)
            {
                return HttpNotFound();
            }
            return View(deposit);
        }

        // POST: deposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            deposit deposit = db.deposits.Find(id);
            db.deposits.Remove(deposit);
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
