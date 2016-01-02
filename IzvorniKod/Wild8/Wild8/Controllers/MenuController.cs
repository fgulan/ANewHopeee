using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;
using Newtonsoft.Json;

namespace Wild8.Controllers
{
    public class MenuController : Controller
    {
        private RestaurauntContext db;

        public MenuController()
        {
            db = new RestaurauntContext();

        }

        // GET: Menu
        public ActionResult Index()
        {
            List<MealWithPrice> mealWPrice = new List<MealWithPrice>();
         
            //If already selected category load that category again
            string cat = (string)(Session["Category"]);
            List<MealWithPrice> meals = new List<MealWithPrice>();
            if (cat == null)
            {
                //TODO: load most wanted meals
                meals = loadMeals("Jela s rostilja");
                cat = "Jela s rostilja";
            }
            else
            {
                meals = loadMeals(cat);
            }
            
            MenuModelView modelView = new MenuModelView()
            {
                activeCategory = cat,
                Meals = meals,
                Categories = loadCategories()
            };
          
            return View(modelView);
        }

        public string ChangeCategory(string categoryName)
        {
            Session["Category"] = categoryName;

            List<MealWithPrice> meals = loadMeals(categoryName);

            return JsonConvert.SerializeObject(meals);
        }

        public void AddToBasket(int mealId, string size, int quantity, string[] addons)
        {

        }

        private List<MealWithPrice> loadMeals(string categry) 
        {
            List<MealWithPrice> mealWPrice = new List<MealWithPrice>();
            foreach (Meal m in db.Meals.Where(meal => meal.Category.Name.Equals(categry)))
            {
                MealWithPrice mwp = new MealWithPrice()
                {
                    Meal = m,
                    Types = db.MealTypes.Where(type => type.MealID == m.MealID).ToList(),
                    IsHot = false   //TODO: change this
                };
                mealWPrice.Add(mwp);
            }

            return mealWPrice;
        }

        private List<string> loadCategories()
        {
            List<string> categories = new List<string>();

            foreach(Category c in db.Categories)
            {
                categories.Add(c.Name);
            }

            return categories;
        }

    }
}