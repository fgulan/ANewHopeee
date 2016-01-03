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
        private RestaurauntContext db = new RestaurauntContext();

        public readonly static string BY_NAME = "Ime (A-Ž)";
        public readonly static string BY_NAME_REVERSE = "Ime (Ž-A)";
        public readonly static string BY_PRICE = "Cijena (najniže prvo)";
        public readonly static string BY_PRICE_REVERSE = "Cijena (najviše prvo)";
        public readonly static string BY_GRADE = "Ocjena (najviše prvo)";
        public readonly static string BY_POPULARITY = "Popularnost (više naručivane)";

        // GET: Menu
        public ActionResult Index()
        {
            List<MealWithPrice> mealWPrice = new List<MealWithPrice>();

            //If already selected category load that category again
            Category activeCategory = (Category)(Session["Category"]);
            List<Category> catgories = db.Categories.ToList();
            if (activeCategory == null)
            {
                activeCategory = catgories.First();
            }
            List<MealWithPrice> meals = loadMeals(activeCategory);
            sortMeals(meals, BY_NAME);

            MenuModelView modelView = new MenuModelView()
            {
                activeCategory = catgories.First(),
                Meals = meals,
                Categories = catgories
            };

            return View(modelView);
        }

        [HttpPost]
        public ActionResult ChangeCategory(int categoryId, string sort)
        {
            List<MealWithPrice> meals = loadMeals(db.Categories.Find(categoryId));
            sortMeals(meals, sort);
            return PartialView("MealsList", meals);
        }

        private List<MealWithPrice> loadMeals(Category category)
        {
            List<MealWithPrice> mealWPrice = new List<MealWithPrice>();
            foreach (Meal m in category.Meals)
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

        private static void sortMeals(List<MealWithPrice> meals, string sort)
        {
            if (sort == BY_NAME)
            {
                meals.Sort((m1, m2) => m1.Meal.Name.CompareTo(m2.Meal.Name));
            }
            else if (sort == BY_NAME_REVERSE)
            {
                meals.Sort((m1, m2) => -m1.Meal.Name.CompareTo(m2.Meal.Name));
            }
            else if (sort == BY_PRICE)
            {
                meals.Sort((m1, m2) => Decimal.Compare(m1.Types.Min().Price, m2.Types.Min().Price));
            }
            else if (sort == BY_PRICE_REVERSE)
            {
                meals.Sort((m1, m2) => -Decimal.Compare(m1.Types.Min().Price, m2.Types.Min().Price));
            }
            else if (sort == BY_GRADE)
            {
                meals.Sort((m1, m2) => m1.Meal.Grade - m2.Meal.Grade);
            }
            else if (sort == BY_POPULARITY)
            {
                meals.Sort((m1, m2) => m1.IsHot ? -1 : 1);
            }
        }
    }
}