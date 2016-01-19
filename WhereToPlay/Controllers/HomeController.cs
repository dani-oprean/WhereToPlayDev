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

            return View(db.Courts.Where(e=>e.Hidden==false).ToList());
        }
    }
}