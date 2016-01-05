using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;

namespace Wild8.Controllers
{
    public class AdminController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Admin
        public ActionResult Index(bool adminRights)
        {
            return View();
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

        public ActionResult AddEditDelMenu()
        {
            //Todo this menu will only be selector for choosing meals
            //to edit or delete and button to create new meal
            //Or maybe another side pane with submenus like add meal, edit meals
            //delete meals
            return PartialView();
        }

        public ActionResult AddMealPartialView()
        {
            return PartialView();
        }

        public ActionResult EditDelMealPartialView(int mealID)
        {
            //Todo parital view for the chosen meal 
            return PartialView("", db.Meals.ToList());
        }
        
        //Every meal has a name and at least one type 
        //Addons are not mandatory
        [HttpPost]
        public void AddMeal(string mealName, string description, int categoryId, string imagePath,
                            string[] typeNames, int[] typePrices, string[] addons) //Addon ids
        {
            Meal meal = new Meal { Name = mealName, Description = description, CategoryID = categoryId, ImagePath = imagePath };
            db.Meals.Add(meal);


            for (int i = 0; i < typeNames.Length; i++)
            {
                var type = new MealType { Meal = meal, MealTypeName = typeNames[i], Price = typePrices[i] };
                db.MealTypes.Add(type);
            }

            if (addons != null)
            {
                for (int i = 0; i < addons.Length; i++)
                {
                    var mealAddon = new MealAddOn { AddOnID = addons[i], Meal = meal };
                    db.MealAddOns.Add(mealAddon);
                }
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Every property that is null will remain the same except addons, if addons are null 
        /// then that meal will have no addons
        /// </summary>
        /// <param name="mealId"></param>
        /// <param name="mealName"></param>
        /// <param name="description"></param>
        /// <param name="categoryId"></param>
        /// <param name="imagePath"></param>
        /// <param name="typeNames"></param>
        /// <param name="typePrices"></param>
        /// <param name="addons"></param>
        [HttpPost]
        public void EditMeal(int mealId, string mealName, string description, int? categoryId, string imagePath,
                             string[] typeNames, int[] typePrices, string[] addons)
        {
            var updatedMeal = new Meal
            {
                MealID = mealId,
                Name = mealName,
                Description = description,
                ImagePath = imagePath
            };
            if(categoryId != null)
            {
                updatedMeal.CategoryID = (int)categoryId;
            }

            db.Meals.Attach(updatedMeal);
            var entry = db.Entry(updatedMeal);  //Change properties
            entry.Property(e => e.Name).IsModified = (mealName == null ? false : true);
            entry.Property(e => e.Description).IsModified = (description == null ? false : true);
            entry.Property(e => e.CategoryID).IsModified = (categoryId == null ? false : true);
            entry.Property(e => e.ImagePath).IsModified = (imagePath == null ? false : true);

            if (typeNames != null)
            {
                //Remove all old types
                //Todo find better solution so that you do not have to call db
                var types = db.MealTypes;
                types.RemoveRange(types.Where(type => type.MealID == mealId));

                //Set new types
                for (int i = 0; i < typeNames.Length; i++)
                {
                    var type = new MealType { MealID = mealId, MealTypeName = typeNames[i], Price = typePrices[i] };
                    types.Add(type);
                }
            }


            //Remove old addons 
            //Todo find better solution so that you do not have to call db
            var mealAddons = db.MealAddOns;
            mealAddons.RemoveRange(mealAddons.Where(mealAddOn => mealAddOn.MealID == mealId));
            if (addons != null)
            {
                //Add new addons
                for (int i = 0; i < addons.Length; i++)
                {
                    mealAddons.Add(new MealAddOn { AddOnID = addons[i], MealID = mealId });
                }
            }

            db.SaveChanges();
        }

        [HttpPost]
        public void DeleteMeal(int mealId)
        {   //Performance wise is awful but sintax is nice
            //To make it better use sql command
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.MealID == mealId));           
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.MealID == mealId));
            db.Meals.Remove(new Meal { MealID = mealId });

            db.SaveChanges();
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
        public ActionResult LogOut()
        {
            //Todo remove user from session

            return RedirectToAction("Index", "Index");
        }
    }
}