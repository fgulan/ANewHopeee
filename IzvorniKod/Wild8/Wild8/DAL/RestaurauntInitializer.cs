﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Models;

namespace Wild8.DAL
{
    public class RestaurauntInitializer : System.Data.Entity.DropCreateDatabaseAlways<RestaurauntContext>
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
                new Meal { Name = "Abc Juha", Description = "najbolji juha u gradu", CategoryID = 1, ImagePath = "dfsf"}
            };
            meals.ForEach(s => context.Meals.Add(s));
            context.SaveChanges();

            var types = new List<MealType>
            {
                new MealType { MealID = 1, MealTypeName = "Mala porcija" ,  Price = 20 },
                new MealType { MealID = 1, MealTypeName = "Velika porcija", Price = 30 },
                new MealType { MealID = 2, MealTypeName = "Mala porcija",   Price = 10 },
                new MealType { MealID = 2, MealTypeName = "Velika porcija", Price = 20 } 
            };
            types.ForEach(t => context.MealTypes.Add(t));
            context.SaveChanges();
            
            var mealAddOns = new List<MealAddOn>
            {
                //Cevapi
                new MealAddOn {MealID = 1, AddOnID = "Kajmak" },
                new MealAddOn {MealID = 1, AddOnID = "Ajvar" },

                //Juha
                new MealAddOn {MealID = 2, AddOnID = "Luk" },
                new MealAddOn {MealID = 2, AddOnID = "Salata" }
            };
            mealAddOns.ForEach(s => context.MealAddOns.Add(s));
            context.SaveChanges();

            var comments = new List<Comment>
            {
                new Comment { Username = "Filip Gulan", Message = "Restoran je the best ono ej.", Grade = 4, CommentDate = DateTime.Now },
                new Comment { Username = "Fredi Saric", Message = "Restorant ti je sranje stari mooj nemilo", Grade = 1, CommentDate = DateTime.Now },
                new Comment { Username = "Matej Janjic", Message = "Smrid ti restoran", Grade = 2, CommentDate = DateTime.Now },
                new Comment { Username = "Josip", Message = "sdsafsafasasfasasfas", Grade = 3 , MealID = 1, CommentDate = DateTime.Now }
            };

            comments.ForEach(s => context.Comments.Add(s));
            context.SaveChanges();
        }
    }
}