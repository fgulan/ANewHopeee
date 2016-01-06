using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Utils;

namespace Wild8.Controllers
{
    public class LoginController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public bool Login(string username, string password)
        {
            username = LoginUtils.sanitize(username);
            password = LoginUtils.sanitize(password);

            Employee employee = db.Employees.Find(username);
            if (employee == null)
            {
                return false;
            }

            if (LoginUtils.SHA256Hash(password) != employee.Password)
            {
                return false;
            }

            SessionExtension.SetUser(Session, employee);

            return true;
        }
    }
}