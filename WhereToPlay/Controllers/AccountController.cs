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
using System.Text;

namespace WhereToPlay.Controllers
{
    public class AccountController : Controller
    {
        private WhereToPlayDb db = new WhereToPlayDb();

        // GET: Account
        public ActionResult Index()
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

            var users = db.Users.ToList();
            foreach(User item in users)
            {
                try
                {
                    item.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == item.UserGroupID).FirstOrDefault();
                }
                catch(System.Data.Entity.Core.EntityCommandExecutionException er)
                {
                    
                }
            }
            
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
                    if (IsPhoneNumber(user.UserPhone))
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
                if(user.UserPassword.Length>20)
                {
                    ModelState.AddModelError("UserPassword", "Campul Parola trebuie sa aiba maxim 20 de caractere!");
                    return View("Register", user);
                }
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
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                            
                        }
                    }
                    return View("Edit", user);
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

            if (user.PasswordChange!="" && user.PasswordChange != null)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                if (user.PasswordChange.Length > 20)
                {
                    ModelState.AddModelError("PasswordChange", "Campul Parola trebuie sa aiba maxim 20 de caractere!");
                    return View("Edit", user);
                }
                editedUser.UserPassword = crypto.Compute(user.PasswordChange);
                editedUser.UserPasswordConfirm = editedUser.UserPassword;
                editedUser.UserPasswordSalt = crypto.Salt;
            }
            else
            {
                editedUser.UserPasswordConfirm = editedUser.UserPassword;
                ModelState.Remove("PasswordChange");
                ModelState.Remove("PasswordChangeConfirm");
            }

            try
            {
                if (db.Entry(editedUser).State == EntityState.Modified)
                {
                    db.SaveChanges();
                }
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
                return View("Edit", user);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
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
                //aici schimb statusul lui Hidden pentru user
                user.Hidden = !(user.Hidden);
                user.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == user.UserGroupID).FirstOrDefault();
                user.UserPasswordConfirm = user.UserPassword;

                //tre sa ii dam hidden si la terenurile lui daca il inactivam si daca e proprietar
                if (user.UserGroup.UserGroupName == "Proprietar" && user.Hidden==true)
                {
                    foreach (Court item in db.Courts.Where(c=>c.CreateUserID==user.IDUser))
                    {
                        item.Hidden = true;
                    }
                }
                db.SaveChanges();
                
            }

            var users = db.Users.ToList();
            foreach (User item in users)
                item.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == item.UserGroupID).FirstOrDefault();
            return View("Index",users);
        }

        // POST: Account/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User usr)
        {
            if (usr.UserName == null || usr.UserPassword == null)
            {
                ModelState.AddModelError("", "Utilizator si parola sunt obligatorii!");
            }
            else
            {
                byte val = ValidateLogin(usr.UserName, usr.UserPassword);
                switch(val)
                {
                    case 0:
                        ModelState.AddModelError("", "Utilizator sau parola sunt gresite!");
                        break;
                    case 1:
                        ModelState.AddModelError("", "Utilizator este inactiv! Va rugam contactati administratorul in pagina Contacteaza-ne.");
                        break;
                    case 2:
                        ModelState.AddModelError("", "Utilizator nu exista in baza de date dar puteti sa il creati!");
                        break;
                    case 3:
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(usr);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }

        [NonAction]
        public byte ValidateLogin(string usrName, string pass)
        {
            var crypto = new SimpleCrypto.PBKDF2();
            var usr = db.Users.Where(e => e.UserName == usrName).FirstOrDefault();
            byte isValid = 0;

            if(usr!=null && !usr.Hidden)
            { 
                if (usr.UserPassword == crypto.Compute(pass, usr.UserPasswordSalt))
                {
                    isValid = 3;
                }
            }
            else
            {
                if (usr == null) isValid = 2;
                else
                    if(usr.Hidden) isValid = 1;
            }
            return isValid;
        }

        [NonAction]
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^[0-9]+$").Success;
        }

        public bool Allowed()
        {
            bool allowed = true;

            User loggedUser = db.Users.Where(u => u.UserName == HttpContext.User.Identity.Name).FirstOrDefault();
            UserGroup UsrGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
            loggedUser.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == loggedUser.UserGroupID).FirstOrDefault();
            loggedUser.UserPasswordConfirm = loggedUser.UserPassword;
            if (UsrGroup.UserGroupName != "Administrator")
            {
                allowed = false;
            }
            return allowed;
        }

        public ActionResult ForgotPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPass(String adresademail)
        {
            User usr = db.Users.Where(u => u.UserEmail == adresademail).FirstOrDefault();
            if (usr!=null)
            {
                String newPass = RandomString(8);
                var crypto = new SimpleCrypto.PBKDF2();
                usr.UserPassword = crypto.Compute(newPass);
                usr.UserPasswordConfirm = usr.UserPassword;
                usr.UserPasswordSalt = crypto.Salt;

                usr.UserGroup = db.UserGroups.Where(u => u.IDUserGroup == usr.UserGroupID).FirstOrDefault();

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
                }

                Utilities.EmailSend(adresademail, "Schimbare parola WhereToPLay", "Salut, noua ta parola pentru userul "+usr.UserName+" este: "+ newPass);
                Utilities.SmsSend(usr.UserPhone, "Salut, noua ta parola pentru userul " + usr.UserName + " este: " + newPass);
                ModelState.AddModelError("", "Un mail/sms cu noua parola v-a fost trimis pe adresa de email "+ adresademail);
                return View("Login");
            }
            else
            {
                ModelState.AddModelError("", "Adresa de email nu a fost gasita!");
                return View();
            }
        }

        [NonAction]
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
