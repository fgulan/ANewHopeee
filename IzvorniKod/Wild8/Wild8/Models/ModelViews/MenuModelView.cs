using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.ModelViews
{
    public class MenuModelView
    {
        public List<Category> Categories { get; set; }
        public List<MealWithPrice> Meals { get; set; }
        public Category activeCategory { get; set; }

    }

    public class MealWithPrice
    {
        public Meal Meal { get; set; }
        public List<MealType> Types { get; set; }
        public Boolean IsHot { get; set; }
    }
}