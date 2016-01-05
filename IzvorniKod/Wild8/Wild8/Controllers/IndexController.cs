using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wild8.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            if(Session["user"] == null)
            {
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}