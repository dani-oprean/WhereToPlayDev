using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhereToPlay.Models;

namespace WhereToPlay.Controllers
{
    public class HomeController : Controller
    {
        private WhereToPlayDb db = new WhereToPlayDb();
        // GET: Home
        public ActionResult Index()
        {
           // Utilities.SmsSend("0764199999", "test din utilities");
           // Utilities.EmailSend("oidldb@gmail.com", "test din utilities", "test din utilities body");
            return View(db.Courts.Where(e=>e.Hidden==false).ToList());
        }

        public ActionResult IndexSports(string id, string terenCautat)
        {
            if (terenCautat == null)
            {
                return View(db.Courts.Where(e => e.Hidden == false).Where(s => s.Sport.SportName == id).ToList());
            }
            else
            {
                var courts = db.Courts.Where(e => e.Hidden == false).Where(s => s.Sport.SportName == id).Where(c => c.CourtName.Contains(terenCautat)).ToList();
                if (courts.Count==0)
                {
                    ModelState.AddModelError(string.Empty, "Ne pare rau, nu am gasit nici un teren!");
                }
                return View(courts);
            }
        }

        public ActionResult NotAllowed()
        {
            return View();
        }

        public ActionResult DespreNoi()
        {
            return View();
        }

        public ActionResult TermeniSiConditii()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(string your_name,string your_email,string your_enquiry)
        {

            if (your_email != null)
            {
                string subject = "Ati primit un mesaj de la " + your_name;
                string mesajBody = your_enquiry + ". Ma puteti contacta la adresa de email: " + your_email;
                Utilities.EmailSend("online.programare@gmail.com", subject, mesajBody);
            }
            return View("Index", db.Courts.Where(e => e.Hidden == false).ToList());
        }
    }
}