using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
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
        [HttpGet]
        public ActionResult Orders()
        {
            return PartialView("Orders");
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
                    try {
                        db.SaveChanges();
                    }
                    catch(Exception ex) {
                        Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        return Content(ex.Message, MediaTypeNames.Text.Plain);
                    }
                }
                return Content("Jelo " + meal.Name +" je dodano u bazu.", MediaTypeNames.Text.Plain);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Jelo nije dodano", MediaTypeNames.Text.Plain);
            }
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
                return PartialView("EditMealList", db.Meals.ToList());
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
            db.Comments.RemoveRange(db.Comments.Where(type => type.MealID == mealId));
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
        [HttpGet]
        public ActionResult AddDeleteAddOnPartial()
        {
            //Todo some parital view for this shit
            return PartialView();
        }

        [HttpPost]
        public ActionResult DeleteAddonPartial()
        {
            return PartialView("", db.AddOns.ToList());
        }

        public ActionResult AddAddOn()
        {
            return PartialView("AddAddOn");
        }

        [HttpPost]
        public ActionResult AddAddOn(string Name, string Price)
        {
            var exists = db.AddOns.Find(Name);
            if(exists != null) //If addon already exists
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Dodatak vec postoji u bazi pod tim imenom", MediaTypeNames.Text.Plain);
            }

            db.AddOns.Add(new AddOn() { AddOnID = Name, Price = Decimal.Parse(Price)});
            db.SaveChanges();

            return Content("Dodatak " + Name + " dodan u bazu", MediaTypeNames.Text.Plain);
        }

        public ActionResult EditAddOn(string id)
        {
            if (id == null)
            {   //Load list of meals in database and render it to user
                return PartialView("EditAddOnList", db.AddOns.ToList());
            }
            AddOn addOn = db.AddOns.Find(id);
            if (addOn == null)
            {
                return HttpNotFound();
            }

            return PartialView("EditAddOn", addOn);
        }

        [HttpPost]
        public bool EditAddOn(string OldName, string Name, string Price)
        {
            AddOn addOn = new AddOn() { AddOnID = Name, Price = Decimal.Parse(Price) };

            if (OldName.Equals(Name))
            {
                db.AddOns.Attach(addOn);
                db.Entry(addOn).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

            var mealAddOns = db.MealAddOns.Where(e => e.AddOnID.Equals(OldName)).ToList();
            db.MealAddOns.RemoveRange(mealAddOns);

            db.AddOns.Add(addOn);
            foreach (var item in mealAddOns)
            {
                db.MealAddOns.Add(new MealAddOn() { AddOnID = Name, MealID = item.MealID });
            }

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
        [HttpGet]
        public ActionResult AddDeleteCategory()
        {

            return PartialView();
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return PartialView("", db.Categories.ToList());
        }

        [HttpGet]
        public ActionResult RemoveCategoryPartial()
        {

            return PartialView("", db.Categories.ToList());
        }

        [HttpPost]
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

        [HttpPost]
        public void RemoveCategory(string categoryName)
        {

        }

        ////////////////////////////////////
        //  Statistic
        ////////////////////////////////////
        [HttpGet]
        public ActionResult Statistic()
        {
            //Load some data
            //Todo make statistic parial veiw
            return PartialView();
        }

        ////////////////////////////////////
        //  Add or delete users
        ////////////////////////////////////
        [HttpGet]
        public ActionResult AddRemoveEmployee()
        {
            //This should some kind of side pan and content veiw 
            return PartialView();
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            //Todo make parital view for adding users
            return PartialView();
        }

        [HttpGet]
        public ActionResult RemoveEmployee()
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
        //  Static info
        ////////////////////////////////////
        [HttpGet]
        public ActionResult StaticInfo()    //Static info view
        {
            return PartialView();
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