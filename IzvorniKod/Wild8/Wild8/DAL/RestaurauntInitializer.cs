using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Models;
using Wild8.Utils;

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
                new Category {Name = "Salate"}
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var meals = new List<Meal>
            {
                new Meal { Name = "Cevapi", Description = "najbolji cevapi u gradu", CategoryID = 1, ImagePath = "sdsa"},
                new Meal { Name = "Abc Juha", Description = "najbolji juha u gradu", CategoryID = 2, ImagePath = "dfsf"}
            };
            meals.ForEach(s => context.Meals.Add(s));
            context.SaveChanges();

            var types = new List<MealType>
            {
                new MealType { Meal = meals[0], MealTypeName = "Mala porcija" ,  Price = 20 },
                new MealType { Meal = meals[0], MealTypeName = "Velika porcija", Price = 30 },
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
                new Comment { Username = "Filip Gulan", Message = "Restoran je the best ono ej.", Grade = 4, CommentDate = DateTime.Now},
                new Comment { Username = "Fredi Saric", Message = "Restorant ti je sranje stari mooj nemilo", Grade = 1, CommentDate = DateTime.Now },
                new Comment { Username = "Matej Janjic", Message = "Smrid ti restoran", Grade = 2, CommentDate = DateTime.Now },
                new Comment { Username = "Josip", Message = "sdsafsafasasfasasfas", Grade = 3 , MealID = 1, CommentDate = DateTime.Now }
            };

            comments.ForEach(s => context.Comments.Add(s));
            context.SaveChanges();

            var employees = new List<Employee>
            {
                new Employee { EmployeeID = "dlatecki", Password = LoginUtils.SHA256Hash("pass_dlatecki"), FirstName = "Domagoj", LastName = "Latečki", Email = "domagoj.latecki@fer.hr", PhoneNumber = "0036478777", Address = "nema", City = "Zagreb", PostCode = "10000", Title = "Code monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "fgulan", Password = LoginUtils.SHA256Hash("pass_fgulan"), FirstName = "Filip", LastName = "Gulan", Email = "filip.gulan@fer.hr", PhoneNumber = "0036479428", Address = "raja", City = "Galovac", PostCode = "23222", Title = "Sync monkey", AdminRights = true, isEmployed=true },
                new Employee { EmployeeID = "fredi", Password = LoginUtils.SHA256Hash("pass_fredi"), FirstName = "Fredi", LastName = "Šarić", Email = "fredi@saric@fer.hr", PhoneNumber = "0036477353", Address = "bez", City = "Zagreb", PostCode = "10000", Title = "Alpha monkey", AdminRights = true, isEmployed=true },
                new Employee { EmployeeID = "majinlizard", Password = LoginUtils.SHA256Hash("pass_majinlizard"), FirstName = "Kenneth", LastName = "Kostrešević", Email = "kenneth.kostreševic@fer.hr", PhoneNumber = "0036482290", Address = "zavičaja", City = "Zagreb", PostCode = "10000", Title = "Doc monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "mjanjic", Password = LoginUtils.SHA256Hash("pass_mjanjic"), FirstName = "Matej", LastName = "Janić", Email = "matej.janjic@fer.hr", PhoneNumber = "0036481160", Address = "nit", City = "Zagreb", PostCode = "10000", Title = "Design monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "tin007", Password = LoginUtils.SHA256Hash("pass_tin007"), FirstName = "Tin", LastName = "Trčak", Email = "tin.trcak@fer.hr", PhoneNumber = "0036460856", Address = "miline", City = "Zagreb", PostCode = "10000", Title = "Doc monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "tyrannizer", Password = LoginUtils.SHA256Hash("pass_tyrannizer"), FirstName = "Jan", LastName = "Kelemen", Email = "jan.kelemen@fer.hr", PhoneNumber = "0036479753", Address = "bez", City = "Varaždin", PostCode = "42000", Title = "Code monkey", AdminRights = false , isEmployed=true},
            };
            employees.ForEach(e => context.Employees.Add(e));
            context.SaveChanges();
        }
    }
}