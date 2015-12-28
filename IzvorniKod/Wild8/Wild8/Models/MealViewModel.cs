using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class MealViewModel
    {
        public string MealPicture { get; set; }

        public string MealName { get; set; }

        public MealType Type { get; set; }

        public ICollection<MealAddOn> AddOns { get; set; }
    }
}