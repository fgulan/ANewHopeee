using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.DAL;
using Wild8.Models;

namespace Wild8.Utils
{
    public class MealUtils
    {
        private static RestaurauntContext db = new RestaurauntContext();

        public static bool IsHot(Meal meal)
        {
            return meal.NumberOfOrders >= averageOrderCount();
        }

        private static int averageOrderCount()
        {
            var sum = 0;
            foreach (Meal meal in db.Meals.ToList())
            {
                sum += meal.NumberOfOrders;
            }

            return sum / db.Meals.Count();
        }
    }
}