using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;

namespace Wild8.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            var user = SessionExtension.GetUser(Session);
            if(user == null)
            {
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}