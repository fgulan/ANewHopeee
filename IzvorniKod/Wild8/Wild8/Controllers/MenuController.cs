using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;

namespace Wild8.Controllers
{
    public class MenuController : Controller
    {
        private RestaurauntContext db;
        private List<string> categoryList;

        public MenuController()
        {
            db = new RestaurauntContext();
            categoryList = new List<string>();
            foreach(Category c in db.Categories)
            {
                categoryList.Add(c.Name);
            }
        }

        // GET: Menu
        public ActionResult Index()
        {
            List<MealWithPrice> mealWPrice = new List<MealWithPrice>();
            MenuModelView modelView = new MenuModelView()
            {
                Categories = categoryList,
                Meals = mealWPrice
            }; 

            //If already selected category load that category again
            string cat = (string)(Session["Category"]);

            IQueryable<Meal> meals = null;
            if(cat == null)
            {
                //Todo: load most wanted meals
                meals = db.Meals.Where(meal => meal.Category.Name == "Jela s rostilja");
            }
            else
            {
                meals = db.Meals.Where(meal => meal.Category.Name == cat);                
            }

            foreach (Meal m in meals)
            {
                MealWithPrice mwp = new MealWithPrice()
                {
                    Meal = m,
                    Types = db.MealTypes.Where(type => type.MealID == m.MealID).ToList(),
                    IsHot = false
                };
         
                mealWPrice.Add(mwp);
            }
            
            return View(modelView);
        }

        public ActionResult ChangeCategory(string categoryName)
        {
            Session["Category"] = categoryName;
            return RedirectToAction("Index");
        }
    }
}