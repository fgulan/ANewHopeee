using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;

namespace Wild8.Controllers
{
    public class LoginController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Login
        public ActionResult Index()
        {
            return RedirectToAction("LoginView");
        }

        public ActionResult Login(string username, string pass)
        {
            //TODO: sanitize user input

            Employee employee = db.Employees.Find(username);
            if (employee == null)
            {
                return HttpNotFound();
            }

            if (SHA256Hash(pass) != employee.Password)
            {
                return HttpNotFound();
            }

            Session["User"] = username;

            return Index();
        }

        public static String SHA256Hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}