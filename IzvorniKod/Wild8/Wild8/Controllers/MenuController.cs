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

        [HttpPost]
        public string ChangeCategory(string categoryName, string sort)
        {
            Session["Category"] = categoryName;
            List<MealWithPrice> meals = loadMeals(categoryName);
            sortMeals(meals, sort);
            string res = JsonConvert.SerializeObject(meals);
            return res;
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

        private void sortMeals(List<MealWithPrice> meals, string sort)
        {
            switch(sort)
            {
                case "Ime (A-Ž)":
                    meals.OrderBy(m => m.Meal.Name); break;
                case "Ime (Ž-A)":
                    meals.OrderByDescending(m => m.Meal.Name); break;
                case "Cijena (najviše prvo)":
                    meals.OrderBy(m => m.Types.Max()); break;
                case "Cijena (najniže prvo)":
                    meals.OrderBy(m => m.Types.Min()); break;
                case "Popularnost (više naručivane)":
                    meals.OrderBy(m => m.IsHot); break;
                case "Ocjena (najviše prvo)":
                    break;
            }
        }
    }
}