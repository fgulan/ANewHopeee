using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.ModelViews
{
    public class AddEditMealModelView
    {
        public Meal Meal { get; set; }
        public IEnumerable<AddOn> AddOns { get; set; }
        public IEnumerable<string> SelectedAddOns { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int SelectedCategory { get; set; }
    }
}