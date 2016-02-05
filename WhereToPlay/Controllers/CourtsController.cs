﻿using System;
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
using WhereToPlay.Models.ViewModel;

namespace WhereToPlay.Controllers
{
    public class CourtsController : Controller
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        // GET: Courts
        public ActionResult Index(string terenCautat)
        {
            if (Request.IsAuthenticated)
            {
                int loggedUserID = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().IDUser;
                var courts = db.Courts.Where(c => c.CreateUserID == loggedUserID).Include(c => c.Address).Include(c => c.Sport).Include(c => c.User);
                if (terenCautat == null)
                {
                    return View(courts.ToList());
                }
                else
                {
                    return View(courts.Where(c => c.CourtName.Contains(terenCautat)).ToList());
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult IndexSports(int id, string terenCautat)
        {
            if (Request.IsAuthenticated)
            {
                int loggedUserID = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault().IDUser;
                var courts = db.Courts.Where(c => c.CreateUserID == loggedUserID).Where(s=>s.SportID==id).Include(c => c.Address).Include(c => c.Sport).Include(c => c.User);
                if(terenCautat==null)
                {
                    return View(courts.ToList());
                }
                else
                {
                    return View(courts.Where(c => c.CourtName.Contains(terenCautat)).ToList());
                }
               
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
            Court court = db.Courts.Where(c => c.IDCourt == id).First();
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
                if (!Allowed())
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
        public ActionResult Create([Bind(Include = "Address,CourtName,Length,Width,PhoneNumber,EmailAddress,SessionPrice, SportID")] Court court, HttpPostedFileBase Content1, HttpPostedFileBase Content2, HttpPostedFileBase Content3)
        //,AddressStreet,AddressNumber,AddressCity,AddressCounty
        {
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

                if (Content1 != null && Content1.ContentLength > 0)
                {
                    byte[] tempFile = new byte[Content1.ContentLength];
                    Content1.InputStream.Read(tempFile, 0, Content1.ContentLength);
                    myCourt.Content1 = tempFile;
                    myCourt.FileName1 = Content1.FileName;
                }


                if (Content2 != null && Content2.ContentLength > 0)
                {
                    byte[] tempFile = new byte[Content2.ContentLength];
                    Content2.InputStream.Read(tempFile, 0, Content2.ContentLength);
                    myCourt.Content2 = tempFile;
                    myCourt.FileName2 = Content2.FileName;
                }


                if (Content3 != null && Content3.ContentLength > 0)
                {
                    byte[] tempFile = new byte[Content3.ContentLength];
                    Content3.InputStream.Read(tempFile, 0, Content3.ContentLength);
                    myCourt.Content3 = tempFile;
                    myCourt.FileName3 = Content3.FileName;
                }

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
            if (Request.IsAuthenticated)
            {
                if (!AllowedEditCourts(id))
                {
                    return RedirectToAction("NotAllowed", "Home");
                }
            }
            else
            {
                return RedirectToAction("NotAllowed", "Home");
            }

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
        public ActionResult Edit([Bind(Include = "IDCourt,CourtName,SportID,AddressID,Length,Width,PhoneNumber,EmailAddress,CreateUserID,SessionPrice")] Court court)
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
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Court court = db.Courts.Find(id);
        //    db.Courts.Remove(court);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Rent(Court court, CourtReservation reservation, string selectedDate)
        {
            Reservation res = new Reservation();
            DateTime reservationDate = Convert.ToDateTime(selectedDate);

            //court
            if (court.IDCourt > 0)
            {
                res.Court = db.Courts.Find(court.IDCourt);
                res.CourtID = court.IDCourt;
            }
            else
            {
                ModelState.AddModelError("", "Teren invalid!");
                return View(res.Court);
            }

            //user
            res.User = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (res.User == null)
            {
                ModelState.AddModelError("", "Utilizator necunoscut!");
                return View(res.Court);
            }
            res.UserID = res.User.IDUser;
            res.User.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == res.User.UserGroupID).FirstOrDefault();
            res.User.UserPasswordConfirm = res.User.UserPassword;

            //date
            if (reservationDate != null &&
                (reservationDate.Year >= DateTime.Now.Year &&
                reservationDate.Month >= DateTime.Now.Month &&
                reservationDate.Day >= DateTime.Now.Day))
            {
                res.ReservationDate = reservationDate;
            }
            else
            {
                ModelState.AddModelError("", "Data invalida! Va rog selectati o data valida din viitor!");
                return View("Details",res.Court);
            }
            bool forToday = (reservationDate.Year == DateTime.Now.Year) &&
                            (reservationDate.Month == DateTime.Now.Month) &&
                            (reservationDate.Day == DateTime.Now.Day);

            //Reservation hours
            if (reservation.NZeceDoispe)
            {
                if ((forToday && DateTime.Now.Hour < 9) || !forToday)
                {
                    Reservation res1012 = Utilities.Clone(res);
                    res1012.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "10-12").First();
                    res1012.ReservationTimeID = res1012.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res1012);
                    db.Entry(res1012.Court).State = EntityState.Detached;
                    db.Entry(res1012.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            if (reservation.NDoispePaispe)
            {
                if ((forToday && DateTime.Now.Hour < 11) || !forToday)
                {
                    Reservation res1214 = Utilities.Clone(res);
                    res1214.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "12-14").First();
                    res1214.ReservationTimeID = res1214.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res1214);
                    db.Entry(res1214.Court).State = EntityState.Detached;
                    db.Entry(res1214.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            if (reservation.NPaispeSaispe)
            {
                if ((forToday && DateTime.Now.Hour < 13) || !forToday)
                {
                    Reservation res1416 = Utilities.Clone(res);
                    res1416.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "14-16").First();
                    res1416.ReservationTimeID = res1416.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res1416);
                    db.Entry(res1416.Court).State = EntityState.Detached;
                    db.Entry(res1416.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            if (reservation.NSaispeOptspe)
            {
                if ((forToday && DateTime.Now.Hour < 15) || !forToday)
                {
                    Reservation res1618 = Utilities.Clone(res);
                    res1618.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "16-18").First();
                    res1618.ReservationTimeID = res1618.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res1618);
                    db.Entry(res1618.Court).State = EntityState.Detached;
                    db.Entry(res1618.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            if (reservation.NOptspeDouazeci)
            {
                if ((forToday && DateTime.Now.Hour < 17) || !forToday)
                {
                    Reservation res1820 = Utilities.Clone(res);
                    res1820.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "18-20").First();
                    res1820.ReservationTimeID = res1820.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res1820);
                    db.Entry(res1820.Court).State = EntityState.Detached;
                    db.Entry(res1820.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            if (reservation.NDouazeciDouajdoi)
            {
                if ((forToday && DateTime.Now.Hour < 19) || !forToday)
                {
                    Reservation res2022 = Utilities.Clone(res);
                    res2022.ReservationTime = db.ReservationTimes.Where(r => r.Hours == "20-22").First();
                    res2022.ReservationTimeID = res2022.ReservationTime.IDReservationTime;
                    db.Reservations.Add(res2022);
                    db.Entry(res2022.Court).State = EntityState.Detached;
                    db.Entry(res2022.User).State = EntityState.Detached;
                }
                else
                {
                    ModelState.AddModelError("", "Terenul trebuie rezervat cu minim o ora inainte de prezentare!");
                    return View("Details", res.Court);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException er)
            {
                foreach (var validationErrors in er.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);

                    }
                }
                return View("Details", res.Court);
            }

            return RedirectToAction("Index", "Home");
        }

        public bool Allowed()
        {
            bool allowed = true;

            User loggedUser = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            UserGroup UsrGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
            loggedUser.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
            loggedUser.UserPasswordConfirm = loggedUser.UserPassword;
            if (UsrGroup.UserGroupName != "Proprietar")
            {
                allowed = false;
            }
            return allowed;
        }

        public bool AllowedEditCourts(int? id)
        {
            bool allowed = true;

            User loggedUser = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            Court CourtLoggedUser = db.Courts.Where(c => (c.User.IDUser == loggedUser.IDUser && c.IDCourt == id)).FirstOrDefault();

            loggedUser.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
            loggedUser.UserPasswordConfirm = loggedUser.UserPassword;
            if (CourtLoggedUser==null)
            {
                allowed = false;
            }
            return allowed;
        }

        //[NonAction]
        public ActionResult ShowTimes(string dateRes, int courtId)
        {
            DateTime date = Convert.ToDateTime(dateRes);
            CourtReservation reservation = new CourtReservation();

            if (Request.IsAjaxRequest())
            {
                var result = db.Reservations
                    .Where(r=>(r.CourtID== courtId) &&
                    (r.ReservationDate.Year==date.Year) && 
                    (r.ReservationDate.Month == date.Month) && 
                    (r.ReservationDate.Day == date.Day))
                    .Select(r=>r.ReservationTime.Hours).ToList();
                
                reservation.NZeceDoispe = result.Contains("10-12");
                reservation.NDoispePaispe = result.Contains("12-14");
                reservation.NPaispeSaispe = result.Contains("14-16");
                reservation.NSaispeOptspe = result.Contains("16-18");
                reservation.NOptspeDouazeci = result.Contains("18-20");
                reservation.NDouazeciDouajdoi = result.Contains("20-22");
            }

            return PartialView("CourtReservationTime",reservation);
        }
    }
}
