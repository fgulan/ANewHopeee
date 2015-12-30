using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Models;

namespace Wild8.DAL
{
    public class RestaurauntInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<RestaurauntContext>
    {
        protected override void Seed(RestaurauntContext context)
        {
            var addOns = new List<AddOn>
            {
                new AddOn { AddOnID = "Kajmak", Price = 3.50M },
                new AddOn { AddOnID = "Ajvar", Price = 3.50M },
                new AddOn { AddOnID = "Majoneza", Price = 3.50M },
                new AddOn { AddOnID = "Ketchup", Price = 3.50M },
                new AddOn { AddOnID = "Luk", Price = 3.50M },
                new AddOn { AddOnID = "Salata", Price = 3.50M },

            };
            addOns.ForEach(s => context.AddOns.Add(s));
            context.SaveChanges();

            var categories = new List<Category>
            {
                new Category {Name = "Jela s rostilja" },
                new Category {Name = "Kuhano" },
                new Category {Name = "Salate" }
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var meals = new List<Meal>
            {
                new Meal { Name = "Cevapi", Description = "najbolji cevapi u gradu", CategoryID = 1, ImagePath = "sdsa"},
                new Meal { Name = "Juha", Description = "najbolji juha u gradu", CategoryID = 2, ImagePath = "dfsf"}
            };
            meals.ForEach(s => context.Meals.Add(s));
            context.SaveChanges();

            var mealAddOns = new List<MealAddOn>
            {
                new MealAddOn {MealID = 1, AddOnID = "Kajmak" },
                new MealAddOn {MealID = 1, AddOnID = "Ajvar" },
                new MealAddOn {MealID = 2, AddOnID = "Luk" },
                new MealAddOn {MealID = 2, AddOnID = "Salata" }
            };
            mealAddOns.ForEach(s => context.MealAddOns.Add(s));
            context.SaveChanges();
        }
    }
}