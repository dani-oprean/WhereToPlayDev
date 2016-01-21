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
using SimpleCrypto;
using System.Diagnostics;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Web.UI;

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
                User vusr = db.Users.Where(u=>u.UserName == user.UserName).FirstOrDefault();
                if(vusr!=null)
                {
                    ModelState.AddModelError("UserName", "Acest nume de utilizator exista deja. Va rog alegeti altul!");
                    return View("Register", user);
                } 
                suser.UserName = user.UserName;
                if (user.UserPhone != null)
                    if(IsPhoneNumber(user.UserPhone))
                    {
                        suser.UserPhone = user.UserPhone;
                    }
                    else
                    {
                        ModelState.AddModelError("UserPhone", "Campul Numar de telefon trebuie sa respecte formatul unui numar de telefon!");
                        return View("Register", user);
                    }
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
                    FormsAuthentication.SetAuthCookie(suser.UserName, false);
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
                return RedirectToAction("Index", "Home");
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
            user.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == user.UserGroupID).FirstOrDefault();
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
        public ActionResult Edit([Bind(Include = "IDUser,UserName,UserPhone,UserEmail,PasswordChange,PasswordChangeConfirm,UserFullName")] User user)
        {
            User editedUser = db.Users.Where(u=>u.IDUser == user.IDUser).FirstOrDefault();
            if (user.UserPhone != null)
                if (IsPhoneNumber(user.UserPhone))
                {
                    editedUser.UserPhone = user.UserPhone;
                }
                else
                {
                    ModelState.AddModelError("UserPhone", "Campul Numar de telefon trebuie sa respecte formatul unui numar de telefon!");
                    return View("Edit", user);
                }
            editedUser.UserEmail = user.UserEmail;
            editedUser.UserFullName = user.UserFullName;
            editedUser.UserGroup = db.UserGroups.Where(u=>u.IDUserGroup == editedUser.UserGroupID).FirstOrDefault();

            
            
            editedUser.UserPasswordConfirm = editedUser.UserPassword;
            
            if (db.GetValidationErrors().ToList().Count==0)
            {
                if(db.Entry(editedUser).State == EntityState.Modified)
                {
                    db.SaveChanges();
                }
            }
            else
            {
                foreach (var validationResults in db.GetValidationErrors())
                {
                    foreach (var error in validationResults.ValidationErrors)
                    {
                        Debug.WriteLine(
                                          "Entity Property: {0}, Error {1}",
                                          error.PropertyName,
                                          error.ErrorMessage);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
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
            else
            {
                //aici schimb statusul lui Hidden
                user.Hidden = !(user.Hidden);
                user.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == user.UserGroupID).FirstOrDefault();
                user.UserPasswordConfirm = user.UserPassword;
                db.SaveChanges();
                
            }
            return View("Index",db.Users);
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
            return RedirectToAction("Index","Home");
        }

        [NonAction]
        public bool ValidateLogin(string usrName, string pass)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var usr = db.Users.Where(e => e.UserName == usrName).FirstOrDefault();
            bool isValid = false;

            if(usr.Hidden==false)
            { 
                if (usr!=null)
                {
                    if (usr.UserPassword == crypto.Compute(pass, usr.UserPasswordSalt))
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
        }

        [NonAction]
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9}").Success;
        }
    }
}
