using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.ModelViews
{
    public class MealModelView
    {
        public MealModelView(Meal meal, List<Comment> comments)
        {
            this.meal = meal;
            this.comments = comments;
        }

        public Meal meal { get; set; }

        public List<Comment> comments { get; set; }
    }
}