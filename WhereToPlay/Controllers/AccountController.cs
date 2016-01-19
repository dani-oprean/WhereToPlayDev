﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WhereToPlay.Models;
using WhereToPlay.Models.DB;
using SimpleCrypto;
using System.Diagnostics;
using System.Web.Security;

namespace WhereToPlay.Controllers
{
    public class AccountController : Controller
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        // GET: Account
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.UserGroup);
            return View(users.ToList());
        }

        // GET: Account/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        // GET: Account/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,UserPhone,UserEmail,UserPassword,UserPasswordConfirm,UserFullName,IAmOwner")] User user)
        {
            ModelState.Remove("UserGroupId");
            ModelState.Remove("UserPasswordSalt");
            if (ModelState.IsValid)
            {
                var crypto = new SimpleCrypto.PBKDF2();

                User suser = new User();
                suser.UserName = user.UserName;
                suser.UserPhone = user.UserPhone;
                suser.UserEmail = user.UserEmail;
                suser.UserFullName = user.UserFullName;
                suser.UserPassword = crypto.Compute(user.UserPassword);
                suser.UserPasswordConfirm = suser.UserPassword;
                suser.UserPasswordSalt = crypto.Salt;
                string owner = user.IAmOwner ? "Proprietar" : "Jucator";
                suser.UserGroupID = db.UserGroups.Where(e => e.UserGroupName == owner).FirstOrDefault().IDUserGroup;
                suser.UserGroup = db.UserGroups.Where(e => e.IDUserGroup == suser.UserGroupID).FirstOrDefault();

                try
                {
                    db.Users.Add(suser);
                    db.SaveChanges();
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException er)
                {
                    foreach (var validationErrors in er.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.UserGroupID = new SelectList(db.UserGroups, "IDUserGroup", "UserGroupName", user.UserGroupID);
            return View("Register",user);
        }

        // GET: Account/Edit/5
        public ActionResult Edit()
        {
            string usrName = User.Identity.Name;
            if (usrName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Where(e=>e.UserName == usrName).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserGroupID = new SelectList(db.UserGroups, "IDUserGroup", "UserGroupName", user.UserGroupID);
            return View(user);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUser,UserName,UserPhone,UserEmail,UserPassword,UserFullName,UserGroupID,Hidden")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserGroupID = new SelectList(db.UserGroups, "IDUserGroup", "UserGroupName", user.UserGroupID);
            return View(user);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User usr)
        {
            if(ValidateLogin(usr.UserName, usr.UserPassword))
            {
                FormsAuthentication.SetAuthCookie(usr.UserName, false);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("","Userul sau parola sunt gresite!");
            }
            return View(usr);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("");
        }

        [NonAction]
        public bool ValidateLogin(string usrName, string pass)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var usr = db.Users.Where(e => e.UserName == usrName).FirstOrDefault();
            bool isValid = false;

            if (usr!=null)
            {
                if (usr.UserPassword == crypto.Compute(pass, usr.UserPasswordSalt))
                {
                    isValid = true;
                }
            }
            return isValid;
        }
    }
}
