using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.ModelViews
{
    public class MenuModelView
    {
        public List<string> categories { get; set; }
        public List<MealWithPrice> meals { get; set; }

    }

    public partial class MealWithPrice
    {
        public Meal meal { get; set; }
        public List<AddOn> addons { get; set; }
        public List<MealType> types { get; set; }
        public Boolean isHot { get; set; }
    }
}