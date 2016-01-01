using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.StaticInfo;

namespace Wild8.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(RestaurauntInfo.Instance);
        }
    }
}