using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
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
            if (Request.IsAuthenticated)
            {
                int loggedUserID = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().IDUser;
                var courts = db.Courts.Where(c => c.CreateUserID == loggedUserID).Include(c => c.Address).Include(c => c.Sport).Include(c => c.User);
                return View(courts.ToList());
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult IndexSports(int id)
        {
            if (Request.IsAuthenticated)
            {
                int loggedUserID = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().IDUser;
                var courts = db.Courts.Where(c => c.CreateUserID == loggedUserID).Where(s=>s.SportID==id).Include(c => c.Address).Include(c => c.Sport).Include(c => c.User);
                return View(courts.ToList());
            }

            return RedirectToAction("Index", "Home");
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
            if (Request.IsAuthenticated)
            {
                User loggedUser = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
                UserGroup UsrGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
                if (UsrGroup.UserGroupName != "Proprietar")
                {
                    return RedirectToAction("NotAllowed", "Home");
                }
            }
            else
            {
                return RedirectToAction("NotAllowed", "Home");
            }

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
        public ActionResult Create([Bind(Include = "Address,CourtName,SportID,AddressID,Length,Width,PhoneNumber,EmailAddress,CreateUserID,SessionPrice")] Court court)
        //,AddressStreet,AddressNumber,AddressCity,AddressCounty
        {
            //ModelState.Remove("AddressID");
            //ModelState.Remove("IDCourt");
            if (ModelState.IsValid)
            {

                Court myCourt = new Court();
                myCourt.CourtName = court.CourtName;
                myCourt.Length = court.Length;
                myCourt.Width = court.Width;
                myCourt.PhoneNumber = court.PhoneNumber;
                myCourt.EmailAddress = court.EmailAddress;
                myCourt.CreateUserID = court.CreateUserID;
                myCourt.SessionPrice = court.SessionPrice;
                //myCourt.Hidden = court.Hidden;

                Address myAdd = new Address();
                myAdd.AddressStreet = court.Address.AddressStreet;
                myAdd.AddressNumber = court.Address.AddressNumber;
                myAdd.AddressCity = court.Address.AddressCity;
                myAdd.AddressCounty = court.Address.AddressCounty;
                db.Addresses.Add(myAdd);
                db.SaveChanges();

                myCourt.AddressID = myAdd.IDAddress;
                myCourt.Address = db.Addresses.Where(a => a.IDAddress == myCourt.AddressID).FirstOrDefault();

                myCourt.SportID = court.SportID;
                myCourt.Sport = db.Sports.Where(s => s.IDSport == myCourt.SportID).FirstOrDefault();

                myCourt.CreateUserID = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().IDUser;
                myCourt.User = db.Users.Where(u => u.IDUser == myCourt.CreateUserID).FirstOrDefault();
                myCourt.User.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == myCourt.User.UserGroupID).FirstOrDefault();
                myCourt.User.UserPasswordConfirm = myCourt.User.UserPassword;

                try
                {
                    db.Courts.Add(myCourt);
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException er)
                {
                    foreach (var validationErrors in er.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",validationError.PropertyName,validationError.ErrorMessage);
                        }
                    }
                }
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
            else
            {
                //aici schimb statusul lui Hidden pentru court
                court.Hidden = !(court.Hidden);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Courts");
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
