using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhereToPlay.Models;
using WhereToPlay.Models.DB;

namespace WhereToPlay.Controllers
{
    public class CourtsController : Controller
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        // GET: Courts
        public ActionResult Index()
        {
            var courts = db.Courts.Include(c => c.Address).Include(c => c.Sport).Include(c => c.User);
            return View(courts.ToList());
        }

        // GET: Courts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            return View(court);
        }

        // GET: Courts/Create
        public ActionResult Create()
        {
            ViewBag.AddressID = new SelectList(db.Addresses, "IDAddress", "AddressStreet");
            ViewBag.SportID = new SelectList(db.Sports, "IDSport", "SportName");
            ViewBag.CreateUserID = new SelectList(db.Users, "IDUser", "UserName");
            return View();
        }

        // POST: Courts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCourt,CourtName,SportID,AddressID,Length,Width,PhoneNumber,EmailAddress,CreateUserID,SessionPrice,Hidden")] Court court)
        {
            if (ModelState.IsValid)
            {
                db.Courts.Add(court);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddressID = new SelectList(db.Addresses, "IDAddress", "AddressStreet", court.AddressID);
            ViewBag.SportID = new SelectList(db.Sports, "IDSport", "SportName", court.SportID);
            ViewBag.CreateUserID = new SelectList(db.Users, "IDUser", "UserName", court.CreateUserID);
            return View(court);
        }

        // GET: Courts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "IDAddress", "AddressStreet", court.AddressID);
            ViewBag.SportID = new SelectList(db.Sports, "IDSport", "SportName", court.SportID);
            ViewBag.CreateUserID = new SelectList(db.Users, "IDUser", "UserName", court.CreateUserID);
            return View(court);
        }

        // POST: Courts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCourt,CourtName,SportID,AddressID,Length,Width,PhoneNumber,EmailAddress,CreateUserID,SessionPrice,Hidden")] Court court)
        {
            if (ModelState.IsValid)
            {
                db.Entry(court).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddressID = new SelectList(db.Addresses, "IDAddress", "AddressStreet", court.AddressID);
            ViewBag.SportID = new SelectList(db.Sports, "IDSport", "SportName", court.SportID);
            ViewBag.CreateUserID = new SelectList(db.Users, "IDUser", "UserName", court.CreateUserID);
            return View(court);
        }

        // GET: Courts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Court court = db.Courts.Find(id);
            if (court == null)
            {
                return HttpNotFound();
            }
            return View(court);
        }

        // POST: Courts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Court court = db.Courts.Find(id);
            db.Courts.Remove(court);
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
