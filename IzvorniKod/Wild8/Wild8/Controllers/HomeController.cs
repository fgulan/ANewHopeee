using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;
using Wild8.StaticInfo;

namespace Wild8.Controllers
{
    public class HomeController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();


        public ActionResult Index()
        {
            List<Meal> meals = db.Meals.OrderByDescending(meal => meal.NumberOfOrders).ToList();
            HomePageModelView modelView = new HomePageModelView()
            {
                FirstMeal = meals[0],
                SecondMeal = meals[1],
                ThirdMeal = meals[2]
            };
            
            //Don't ask why is this here
            RestaurauntInfo instance = RestaurauntInfo.Instance;
            if (instance.RestourantGrade == -1)
            {
                instance.RestourantGrade = Convert.ToDecimal(db.Comments.Average(c => c.Grade));
                instance.SaveData();
            }

            return View(modelView);
        }
    }
}