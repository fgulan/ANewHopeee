using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;
using Wild8.Utils;

namespace Wild8.Controllers
{
    public class AdminController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Admin
        public ActionResult Index()
        {
            var user = SessionExtension.GetUser(Session);
            if(user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        ////////////////////////////////////
        //  Orders
        ////////////////////////////////////
        [HttpPost]
        public ActionResult Orders()
        {
            //Todo make orders parital view
            List<Order> orders = db.Orders.Where(order => order.AcceptanceDate != null).ToList();

            return PartialView("", orders);
        }

        ////////////////////////////////////
        //  Meal add, edit, delete
        ////////////////////////////////////
        [HttpPost]
        public void AcceptOrder(int orderId)
        {
            var order = new Order() { OrderID = orderId , AcceptanceDate = DateTime.Now};

            db.Orders.Attach(order);
            db.Entry(order).Property(e => e.AcceptanceDate).IsModified = true;

            db.SaveChanges();
        }

        [HttpGet]
        public ActionResult AddEditDelMenu()
        {
            //Todo this menu will only be selector for choosing meals
            //to edit or delete and button to create new meal
            //Or maybe another side pane with submenus like add meal, edit meals
            //delete meals
            return PartialView("AddEditDelMealsPartial");
        }

        [HttpPost]
        public ActionResult AddMeal([Bind(Include = "MealID,Name,Description,CategoryID")] Meal meal, IEnumerable<string> SelectedAddOns, HttpPostedFileBase upload, string[] MealType, string[] Price)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetFileName(upload.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/images/Meals"), fileName);
                    string sourcePath = "images/Meals/" + fileName;
                    upload.SaveAs(physicalPath);
                    meal.ImagePath = sourcePath;
                }
                meal = db.Meals.Add(meal);
                db.SaveChanges();

                if (SelectedAddOns != null)
                {
                    foreach (var item in SelectedAddOns)
                    {
                        db.MealAddOns.Add(new MealAddOn()
                        {
                            AddOnID = item,
                            MealID = meal.MealID
                        });
                    }
                }

                if (MealType != null && Price != null)
                {
                    for (int i = 0; i < MealType.Length; i++)
                    {
                        string mealTypeName = MealType[i];
                        string priceString = Price[i];
                        if (mealTypeName != null && mealTypeName.Length > 0  && priceString != null && priceString.Length > 0 )
                        {
                            db.MealTypes.Add(new MealType()
                            {
                                MealID = meal.MealID,
                                MealTypeName = MealType[i],
                                Price = decimal.Parse(Price[i], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                    db.SaveChanges();
                }
            }
            return Content("Error");
        }

        [HttpGet]
        public ActionResult AddMeal()
        {
            AddEditMealModelView newMeal = new AddEditMealModelView()
            {
                Categories = db.Categories.ToList(),
                AddOns = db.AddOns.ToList()
            };
            return PartialView("AddMeal",newMeal);
        }

        [HttpGet]
        public ActionResult EditMeal(int? id)
        {
            if (id == null)
            {   //Load list of meals in database and render it to user
                return View("EditMealList", db.Meals.ToList());
            }
            Meal meal = db.Meals.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            List<string> addOns = new List<string>();
            foreach (var item in meal.AddOns)
            {
                addOns.Add(item.AddOn.AddOnID);
            }

            var mealTypes = db.MealTypes.Where(r => r.MealID == meal.MealID);

            AddEditMealModelView newMeal = new AddEditMealModelView()
            {
                Categories = db.Categories.ToList(),
                AddOns = db.AddOns.ToList(),
                Meal = meal,
                SelectedCategory = meal.CategoryID,
                SelectedAddOns = addOns,
                MealTypes = mealTypes
            };

            return PartialView("EditMeal",newMeal);
        }

        [HttpPost]
        public void EditMeal([Bind(Include = "MealID,Name,Description,CategoryID")] Meal meal, IEnumerable<string> SelectedAddOns, HttpPostedFileBase upload, string[] MealType, string[] Price)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetFileName(upload.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/images/Meals"), fileName);
                    string sourcePath = "images/Meals/" + fileName;
                    upload.SaveAs(physicalPath);
                    meal.ImagePath = sourcePath;
                }
                db.Entry(meal).State = EntityState.Modified;
                db.SaveChanges();

                var mealAddons = db.MealAddOns;
                mealAddons.RemoveRange(mealAddons.Where(mealAddOn => mealAddOn.MealID == meal.MealID));
                if (SelectedAddOns != null)
                {
                    foreach (var item in SelectedAddOns)
                    {
                        MealAddOn mealAddOn = new MealAddOn()
                        {
                            AddOnID = item,
                            MealID = meal.MealID
                        };
                        db.MealAddOns.Add(mealAddOn);
                    }
                }

                var types = db.MealTypes;
                types.RemoveRange(types.Where(type => type.MealID == meal.MealID));
                if (MealType != null && Price != null)
                {
                    for (int i = 0; i < MealType.Length; i++)
                    {
                        string mealTypeName = MealType[i];
                        string priceString = Price[i];
                        if (mealTypeName != null && mealTypeName.Length > 0 && priceString != null && priceString.Length > 0)
                        {
                            db.MealTypes.Add(new MealType()
                            {
                                MealID = meal.MealID,
                                MealTypeName = MealType[i],
                                Price = decimal.Parse(Price[i], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void DeleteMeal(int mealId)
        {   //Performance wise is awful but sintax is nice
            //To make it better use sql command
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.MealID == mealId));           
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.MealID == mealId));
            var meal = new Meal { MealID = mealId};
            db.Meals.Attach(meal);
            db.Meals.Remove(meal);
            db.SaveChanges();
        }

        [HttpGet]
        public ActionResult DeleteMeal()
        {
            return PartialView("DelMeals", db.Meals.ToList());
        }

        ////////////////////////////////////
        //  Add delete addon
        ////////////////////////////////////
        [HttpPost]
        public ActionResult AddDeleteAddOnPartial()
        {
            //Todo some parital view for this shit
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddAddonParitial()
        {

            return PartialView("", db.AddOns.ToList());
        }

        [HttpPost]
        public ActionResult DeleteAddonPartial()
        {
            return PartialView("", db.AddOns.ToList());
        }

        [HttpPost]
        public bool AddAddon(string addOnName, string price)
        {
            var exists = db.AddOns.Find(addOnName);
            if(exists != null) //If addon already exists
            {
                return false;
            }

            db.AddOns.Add(new AddOn() { AddOnID = addOnName, Price = Decimal.Parse(price) });
            db.SaveChanges();

            return true;
        }

        [HttpPost]
        public void RemoveAddon(string addOnName)
        {
            db.AddOns.Remove(new AddOn() { AddOnID = addOnName});
            db.SaveChanges();
        }

        ////////////////////////////////////
        //  Add delete Category
        ////////////////////////////////////
        [HttpPost]
        public ActionResult AddDeleteCategoryPartial()
        {


            return PartialView();
        }

        public ActionResult AddCategoryPartial()
        {

            return PartialView("", db.Categories.ToList());
        }

        public ActionResult RemoveCategoryPartial()
        {

            return PartialView("", db.Categories.ToList());
        }

        public bool AddCategory(string categoryName)
        {
            var exists = db.Categories.First(cat => cat.Name == categoryName);     
            if(exists != null)
            {
                return false;
            }

            db.Categories.Add(new Category { Name = categoryName });
            db.SaveChanges();

            return true;
        }

        public void RemoveCategory(string categoryName)
        {
            db.Categories.Remove(new Category { Name = categoryName });
            db.SaveChanges();
        }

        ////////////////////////////////////
        //  Statistic
        ////////////////////////////////////
        [HttpPost]
        public ActionResult Statistic()
        {
            //Load some data
            //Todo make statistic parial veiw
            return PartialView();
        }

        ////////////////////////////////////
        //  Add or delete users
        ////////////////////////////////////
        public ActionResult AddRemoveUserPartial()
        {
            //This should some kind of side pan and content veiw 
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddUserPartial()
        {
            //Todo make parital view for adding users
            return PartialView();
        }

        public ActionResult RemoveUserParital()
        {
            //Todo make remove user parital View
            return PartialView("", db.Employees.Where(e => !e.AdminRights).ToList());
        }

        public void AddEmployee(string employeeId, string pass, 
                            string firstName, string lastName,
                            string email, string phoneNumber,
                            string address, string city, string postCode,
                            string title)
        {
            //Todo add hashed pass
            Employee e = new Employee
            {
                EmployeeID = employeeId,
                Password = LoginUtils.SHA256Hash(pass),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                PostCode = postCode,
                Title = title,
                AdminRights = false
            };

            db.Employees.Add(e);
            db.SaveChanges();
        }

        public void RemoveEmployee(string employeeId)
        {
            db.Employees.Remove(new Employee() { EmployeeID = employeeId});
            db.SaveChanges();
        }


        ////////////////////////////////////
        //  Logout
        ////////////////////////////////////
        public void LogOut()
        {
            SessionExtension.SetUser(Session, null);
        }
    }
}