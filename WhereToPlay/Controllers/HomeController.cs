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

        public ActionResult IndexSports(int id)
        {
            return View(db.Courts.Where(e => e.Hidden == false).Where(s=>s.SportID==id).ToList());
        }
    }
}