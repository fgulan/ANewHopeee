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
                new AddOn { AddOnID = "Lepinja", Price = 10M },
                new AddOn { AddOnID = "Ajvar", Price = 5M },
                new AddOn { AddOnID = "Luk", Price = 5M },
                new AddOn { AddOnID = "Pomfrit", Price = 6M },
                new AddOn { AddOnID = "Kroketi", Price = 6M },

                new AddOn { AddOnID = "Gauda", Price = 7M },
                new AddOn { AddOnID = "Mozzarela", Price = 7M },
                new AddOn { AddOnID = "Gorgonzola", Price = 7M },
                new AddOn { AddOnID = "Parmezan", Price = 7M },
                new AddOn { AddOnID = "Masline", Price = 7M },
            };
            addOns.ForEach(s => context.AddOns.Add(s));
            context.SaveChanges();

            generateGrill(context);
            generatePizzas(context);
            generateSandwitches(context);
            generateSalads(context);

            var comments = new List<Comment>
            {
                new Comment { Username = "Filip Gulan", Message = "Restoran je the best ono ej.", Grade = 4, CommentDate = DateTime.Now},
                new Comment { Username = "Fredi Saric", Message = "Restorant ti je sranje stari mooj nemilo", Grade = 1, CommentDate = DateTime.Now },
                new Comment { Username = "Matej Janjic", Message = "Smrid ti restoran", Grade = 2, CommentDate = DateTime.Now },
                new Comment { Username = "Josip", Message = "sdsafsafasasfasasfas", Grade = 3 , MealID = 1, CommentDate = DateTime.Now },
                new Comment { Username = "Superhik", Message = "Nema lošeg vina s češnjakom. 1/10 smeće totalno.", Grade = 1, CommentDate = DateTime.Now }
            };

            comments.ForEach(s => context.Comments.Add(s));
            context.SaveChanges();

            var employees = new List<Employee>
            {
                new Employee { EmployeeID = "dlatecki", Password = TextUtils.SHA256Hash("pass_dlatecki"), FirstName = "Domagoj", LastName = "Latečki", Email = "domagoj.latecki@fer.hr", PhoneNumber = "0036478777", Address = "nema", City = "Zagreb", PostCode = "10000", Title = "Code monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "fgulan", Password = TextUtils.SHA256Hash("pass_fgulan"), FirstName = "Filip", LastName = "Gulan", Email = "filip.gulan@fer.hr", PhoneNumber = "0036479428", Address = "raja", City = "Galovac", PostCode = "23222", Title = "Sync monkey", AdminRights = true, isEmployed=true },
                new Employee { EmployeeID = "fredi", Password = TextUtils.SHA256Hash("pass_fredi"), FirstName = "Fredi", LastName = "Šarić", Email = "fredi@saric@fer.hr", PhoneNumber = "0036477353", Address = "bez", City = "Zagreb", PostCode = "10000", Title = "Alpha monkey", AdminRights = true, isEmployed=true },
                new Employee { EmployeeID = "majinlizard", Password = TextUtils.SHA256Hash("pass_majinlizard"), FirstName = "Kenneth", LastName = "Kostrešević", Email = "kenneth.kostreševic@fer.hr", PhoneNumber = "0036482290", Address = "zavičaja", City = "Zagreb", PostCode = "10000", Title = "Doc monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "mjanjic", Password = TextUtils.SHA256Hash("pass_mjanjic"), FirstName = "Matej", LastName = "Janić", Email = "matej.janjic@fer.hr", PhoneNumber = "0036481160", Address = "nit", City = "Zagreb", PostCode = "10000", Title = "Design monkey", AdminRights = false, isEmployed=false },
                new Employee { EmployeeID = "tin007", Password = TextUtils.SHA256Hash("pass_tin007"), FirstName = "Tin", LastName = "Trčak", Email = "tin.trcak@fer.hr", PhoneNumber = "0036460856", Address = "miline", City = "Zagreb", PostCode = "10000", Title = "Doc monkey", AdminRights = false, isEmployed=true },
                new Employee { EmployeeID = "tyrannizer", Password = TextUtils.SHA256Hash("pass_tyrannizer"), FirstName = "Jan", LastName = "Kelemen", Email = "jan.kelemen@fer.hr", PhoneNumber = "0036479753", Address = "bez", City = "Varaždin", PostCode = "42000", Title = "Code monkey", AdminRights = false , isEmployed=true},
            };
            employees.ForEach(e => context.Employees.Add(e));
            context.SaveChanges();

            var orders = new List<Order>
            {
                new Order { AcceptanceDate = new DateTime(2015, 1, 1), TotalPrice = 220.00M, Address = "adr", UserNote = "atn",  Email = "mail", EmpolyeeID = "dlatecki", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr", OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 10, MealName = "Ćevapi", MealType = "Mala porcija", OrderID = 1  },
                    new OrderDetail { Count = 1, MealName = "Nesto", MealType = "Mala porcija", OrderID = 1 }
                } },
                new Order { AcceptanceDate = new DateTime(2015, 1, 1), TotalPrice = 50.00M, Address = "adr",  UserNote = "atn", Email = "mail", EmpolyeeID = "fgulan", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr",OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 2},
                    new OrderDetail { Count = 1, MealName = "Svinjska vratina", MealType = "Mala porcija", OrderID = 2}
                } },
                new Order { AcceptanceDate = new DateTime(2015, 3, 1), TotalPrice = 70.00M, Address = "adr", UserNote = "atn",  Email = "mail", EmpolyeeID = "fredi", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr",  OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 3},
                    new OrderDetail { Count = 2, MealName = "Svinjska vratina", MealType = "Mala porcija", OrderID = 3 }
                } },
                new Order { AcceptanceDate = new DateTime(2015, 4, 1), TotalPrice = 50.00M, Address = "adr", UserNote = "atn",  Email = "mail", EmpolyeeID = "majinlizard", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr",  OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Jumbo", OrderID = 4 },
                    new OrderDetail { Count = 1, MealName = "Svinjska vratina", MealType = "Mala porcija", OrderID = 4 }
                } },
                new Order { AcceptanceDate = new DateTime(2015, 5, 1), TotalPrice = 40.00M, Address = "adr",  UserNote = "atn",  Email = "mail", EmpolyeeID = "mjanjic", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr",  OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Srednja", OrderID = 4 },
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 4 }
                } },
                new Order { AcceptanceDate = new DateTime(2015, 5, 1), TotalPrice = 40.00M, Address = "adr",  UserNote = "atn",  Email = "mail", EmpolyeeID = "mjanjic", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr", OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Mala", OrderID = 4},
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 4}
                } },
                new Order { AcceptanceDate = new DateTime(2015, 5, 1), TotalPrice = 40.00M, Address = "adr",UserNote = "atn",  Email = "mail", EmpolyeeID = "mjanjic", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr", OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Jumbo", OrderID = 4 },
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 4 }
                } },
                new Order { AcceptanceDate = new DateTime(2015, 5, 1), TotalPrice = 40.00M, Address = "adr", UserNote = "atn", Email = "mail", EmpolyeeID = "mjanjic", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr", OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Mala", OrderID = 4 },
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 4}
                } },
                new Order { AcceptanceDate = new DateTime(2015, 5, 1), TotalPrice = 40.00M, Address = "adr", UserNote = "atn", Email = "mail", EmpolyeeID = "mjanjic", Name = "name", OrderDate = DateTime.Now, PhoneNumber = "nmbr",  OrderDetails = new List<OrderDetail> {
                    new OrderDetail { Count = 1, MealName = "Slavonska", MealType = "Srednja", OrderID = 4 },
                    new OrderDetail { Count = 1, MealName = "Ražnjiči", MealType = "Mala porcija", OrderID = 4 }
                } }
            };
            orders.ForEach(e => context.Orders.Add(e));
            context.SaveChanges();
        }

        private void generatePizzas(RestaurauntContext context)
        {
            Category cat = new Category { Name = "Pizze" };
            var pizzas = new List<Meal>
            {
                new Meal { Name = "Margharita", NumberOfOrders = 0, Description = "Rajčica, sir", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Funghi", NumberOfOrders = 0, Description = "Rajčica, sir, šampinjoni", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Vesuvio", NumberOfOrders = 0, Description = "Rajčica, sir, šunka", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Capricciosa", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, šampinjoni", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Quattro formaggio", NumberOfOrders = 0, Description = "Rajčica, 4 vrste sira", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Al tonno", NumberOfOrders = 0, Description = "Rajčica, sir, tunjevina", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Frutti di mare", NumberOfOrders = 0, Description = "Rajčica, sir, plodovi mora", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Vegetariana", NumberOfOrders = 0, Description = "Rajčica, sir, paprika, kukuruz, šampinjoni", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Prosccuiuto", NumberOfOrders = 0, Description = "Rajčica, sir, pršut", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Slavonska", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, šampinjoni, kulen, špek", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Napolitana", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, šampinjoni, špek, jaje", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Mexicana", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, špek, kukuruz, tabasco, paprika", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Picante", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, špek, feferoni", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Zagrebačka", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, šampinjoni, svj. paprika, kiselo vrhnje, špek", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Asterix", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, špek, kulen, jaje", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Lovačka", NumberOfOrders = 0, Description = "Rajčica, sir, šunka, šampinjoni, kulen, ljuti feferoni", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Baranjska", NumberOfOrders = 0, Description = "Rajčica, sir, šunk,a kulen, ljuti feferoni, luk, špek", Category = cat, ImagePath = "todo", IsAvailable = true }
            };
            pizzas.ForEach(s => context.Meals.Add(s));

            foreach (Meal m in pizzas)
            {
                var pizzaTypes = new List<MealType>
                {
                    new MealType { Meal = m, MealTypeName = "Mala", Price = 30 },
                    new MealType { Meal = m, MealTypeName = "Srednja", Price = 35 },
                    new MealType { Meal = m, MealTypeName = "Jumbo", Price = 70 }
                };
                pizzaTypes.ForEach(t => context.MealTypes.Add(t));

                var pizzaAddOns = new List<MealAddOn>
                {
                    new MealAddOn { Meal = m, AddOnID = "Gauda" },
                    new MealAddOn { Meal = m, AddOnID = "Mozzarela"},
                    new MealAddOn { Meal = m, AddOnID = "Gorgonzola"},
                    new MealAddOn { Meal = m, AddOnID = "Parmezan"},
                    new MealAddOn { Meal = m, AddOnID = "Masline"},
                };
                pizzaAddOns.ForEach(a => context.MealAddOns.Add(a));
            }
            context.SaveChanges();
        }

        private void generateGrill(RestaurauntContext context)
        {
            Category cat = new Category { Name = "Jela s roštilja" };
            var grill = new List<Meal>
        {
                new Meal { Name = "Ćevapi", NumberOfOrders = 0, Description = "", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Pljeskavica", NumberOfOrders = 0, Description = "", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Pljeskavica punjena", NumberOfOrders = 0, Description = "", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Ražnjiči", NumberOfOrders = 0, Description = "Svinjsko meso", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Ražnjiči", NumberOfOrders = 0, Description = "Pileće meso", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Svinjska vratina", NumberOfOrders = 0, Description = "", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Naravni", NumberOfOrders = 0, Description = "Svinjsko meso", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Naravni", NumberOfOrders = 0, Description = "Pileće meso", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Miješano meso", NumberOfOrders = 0, Description = "", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Punjeni lungić", NumberOfOrders = 0, Description = "Sir, šunka", Category = cat, ImagePath = "todo", IsAvailable = true },
        };
            grill.ForEach(s => context.Meals.Add(s));

            foreach (Meal m in grill)
            {
                var grillTypes = new List<MealType>
                {
                    new MealType { Meal = m, MealTypeName = "Mala porcija", Price = 30 },
                    new MealType { Meal = m, MealTypeName = "Velika porcija", Price = 45},
                };
                grillTypes.ForEach(t => context.MealTypes.Add(t));

                var grillAddOns = new List<MealAddOn>
                {
                    new MealAddOn { Meal = m, AddOnID = "Lepinja" },
                    new MealAddOn { Meal = m, AddOnID = "Ajvar"},
                    new MealAddOn { Meal = m, AddOnID = "Luk"},
                    new MealAddOn { Meal = m, AddOnID = "Pomfrit"},
                    new MealAddOn { Meal = m, AddOnID = "Kroketi"},
                };
                grillAddOns.ForEach(a => context.MealAddOns.Add(a));
            }
            context.SaveChanges();
        }

        private void generateSandwitches(RestaurauntContext context)
        {
            Category cat = new Category { Name = "Sendviči" };
            var sandwitches = new List<Meal>
            {
                new Meal { Name = "Šunka", NumberOfOrders = 0, Description = "Domaća lepinja, sir, šunka, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Kvočko", NumberOfOrders = 0, Description = "Domaća lepinja, piletina, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Kulen", NumberOfOrders = 0, Description = "Domaća lepinja, sir, kulen, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Vratina", NumberOfOrders = 0, Description = "Domaća lepinja, sir, vratina, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Pršut", NumberOfOrders = 0, Description = "Domaća lepinja, sir, pršut, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Tuna", NumberOfOrders = 0, Description = "Domaća lepinja, sir, tuna, povrće, umak", Category = cat, ImagePath = "todo", IsAvailable = true }
            };

            foreach (Meal m in sandwitches)
            {
                var sandwitchTypes = new List<MealType>
                {
                    new MealType { Meal = m, MealTypeName = "Mali", Price = 15 },
                    new MealType { Meal = m, MealTypeName = "Veliki", Price = 25 },
                };
                sandwitchTypes.ForEach(t => context.MealTypes.Add(t));

                var sandwitchAddOns = new List<MealAddOn>
                {
                    new MealAddOn { Meal = m, AddOnID = "Ajvar"},
                    new MealAddOn { Meal = m, AddOnID = "Luk"},
                    new MealAddOn { Meal = m, AddOnID = "Pomfrit"},
                };
                sandwitchAddOns.ForEach(a => context.MealAddOns.Add(a));
            }
            context.SaveChanges();
        }

        private void generateSalads(RestaurauntContext context)
        {
            Category cat = new Category { Name = "Salate" };
            var salads = new List<Meal>
            {
                new Meal { Name = "Pollo", NumberOfOrders = 0, Description = "Miješana salata s piletnom sa žara, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Tonno", NumberOfOrders = 0, Description = "Tjestenina, miješana salata, tuna, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Piletina", NumberOfOrders = 0, Description = "Tjestenina, piletina sa žara, kukur, paprika ,dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Meksička", NumberOfOrders = 0, Description = "Tjestanina, grah, kukuruz, luk, piletina, paprika ,ljuti dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Gurmanska", NumberOfOrders = 0, Description = "Tjestenina, sir, šunka, paprika, rajčica, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Kraljevska", NumberOfOrders = 0, Description = "Piletina, paprika, rajčica, zelena salata, kuhano jaje, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Grčka", NumberOfOrders = 0, Description = "Zelena salata, paprika, rajčcia, sir, mozzarela, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
                new Meal { Name = "Šefova", NumberOfOrders = 0, Description = "Miješana salata, piletina, mozzarela, prženi špek, dresing", Category = cat, ImagePath = "todo", IsAvailable = true },
            };

            foreach (Meal m in salads)
            {
                var saladTypes = new List<MealType>
                {
                    new MealType { Meal = m, MealTypeName = "Mala", Price = 10 },
                    new MealType { Meal = m, MealTypeName = "Velika", Price = 25 },
                };
                saladTypes.ForEach(t => context.MealTypes.Add(t));

                var saladAddOns = new List<MealAddOn>
                {
                    new MealAddOn { Meal = m, AddOnID = "Masline"},
                    new MealAddOn { Meal = m, AddOnID = "Luk"},
                };
                saladAddOns.ForEach(a => context.MealAddOns.Add(a));
            }
            context.SaveChanges();
        }
    }
}