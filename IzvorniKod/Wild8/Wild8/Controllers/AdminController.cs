using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Newtonsoft.Json;
using Wild8.StaticInfo;

namespace Wild8.Controllers
{
    public class AdminController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Admin
        public ActionResult Index()
        {
            var user = SessionExtension.GetUser(Session);
            if (user == null)
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
            return PartialView("Orders/Orders");
        }

        ////////////////////////////////////
        //  Meal add, edit, delete
        ////////////////////////////////////
        [HttpPost]
        public void AcceptOrder(string orderJSON, string employeeId, string message)
        {
            var acceptedOrder = JsonConvert.DeserializeObject<Order>(orderJSON);
            acceptedOrder.EmpolyeeID = employeeId;
            acceptedOrder.AcceptanceDate = new DateTime();

            OrderInfo info = new OrderInfo()
            {
                Info = RestaurauntInfo.Instance,
                Message = message,
                Order = acceptedOrder
            };

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "Receipt");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                var receipt = sw.ToString();

                MailUtil.SendReceiptTo(acceptedOrder.Email, "Potvrda narudžbe",receipt);
            }

            db.Orders.Add(acceptedOrder);
            db.SaveChanges();
        }

        [HttpPost]
        public void RefuseOrder(string email, string message)
        {
            MailUtil.SendReceiptTo(email, "Naruđba odbijena", message);
        }

        [HttpGet]
        public ActionResult AddEditDelMenu()
        {
            return PartialView("AddEditDelMealsPartial");
        }

        [HttpGet]
        public ActionResult AddMeal()
        {
            AddEditMealModelView newMeal = new AddEditMealModelView()
            {
                Categories = db.Categories.ToList(),
                AddOns = db.AddOns.ToList()
            };
            return PartialView("AddMeal", newMeal);
        }

        [HttpPost]
        public ActionResult AddMeal([Bind(Include = "MealID,Name,Description,CategoryID,IsAvailable")] Meal meal, IEnumerable<string> SelectedAddOns, HttpPostedFileBase upload, string[] MealType, string[] Price)
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
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Content("Jelo " + meal.Name + " već postoji.", MediaTypeNames.Text.Plain);
                    }
                }
                return Content("Jelo " + meal.Name + " je spremljeno.", MediaTypeNames.Text.Plain);
            }
            else
            {
                return Content("Neočekivana greška.", MediaTypeNames.Text.Plain);
            }
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

            return PartialView("EditMeal", newMeal);
        }

        [HttpPost]
        public ActionResult EditMeal([Bind(Include = "MealID,Name,Description,CategoryID,ImagePath,IsAvailable")] Meal meal, IEnumerable<string> SelectedAddOns, HttpPostedFileBase upload, string[] MealType, string[] Price)
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
                return Content("Jelo " + meal.Name + " je spremljeno.", MediaTypeNames.Text.Plain);
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Content("Jelo nije spremljeno.", MediaTypeNames.Text.Plain);
            }
        }

        [HttpGet]
        public ActionResult DeleteMeal()
        {
            return PartialView("DelMeals", db.Meals.ToList());
        }

        [HttpPost]
        public void DeleteMeal(int mealId)
        {   //Performance wise is awful but sintax is nice
            //To make it better use sql command
            db.Comments.RemoveRange(db.Comments.Where(type => type.MealID == mealId));
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.MealID == mealId));
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.MealID == mealId));
            var meal = new Meal { MealID = mealId };
            db.Meals.Attach(meal);
            db.Meals.Remove(meal);
            db.SaveChanges();
        }

        ////////////////////////////////////
        //  Add delete addon
        ////////////////////////////////////
        public ActionResult AddAddOn()
        {
            return PartialView("AddAddOn");
        }

        [HttpPost]
        public ActionResult AddAddOn(string Name, string Price)
        {
            var exists = db.AddOns.Find(Name);
            if (exists != null) //If addon already exists
            {
                return Content("Dodatak već postoji u bazi pod tim imenom", MediaTypeNames.Text.Plain);
            }
            Price = Price.Replace('.', ',');
            db.AddOns.Add(new AddOn() { AddOnID = Name, Price = Decimal.Parse(Price) });
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
        public ActionResult EditAddOn(string OldName, string Name, string Price)
        {
            Price = Price.Replace('.', ',');
            AddOn addOn = new AddOn() { AddOnID = Name, Price = Decimal.Parse(Price) };

            if (OldName.Equals(Name))
            {
                db.AddOns.Attach(addOn);
                db.Entry(addOn).State = EntityState.Modified;
                db.SaveChanges();
                return Content("Dodatak " + addOn.AddOnID + " uspješno spremljen.", MediaTypeNames.Text.Plain);
            }

            var query = from mealAddOn in db.MealAddOns
                        where mealAddOn.AddOnID.Equals(OldName)
                        select mealAddOn;
            var mealAddOns = query.ToList();
            db.MealAddOns.RemoveRange(mealAddOns);
            db.SaveChanges();

            AddOn addOnToDel = db.AddOns.Find(OldName);
            db.AddOns.Remove(addOnToDel);

            db.AddOns.Add(addOn);
            foreach (var item in mealAddOns)
            {
                db.MealAddOns.Add(new MealAddOn() { AddOnID = Name, MealID = item.MealID });
            }

            db.SaveChanges();
            return Content("Dodatak " + addOn.AddOnID + " uspješno spremljen.", MediaTypeNames.Text.Plain);
        }

        [HttpGet]
        public ActionResult DeleteAddon()
        {
            return PartialView("DelAddOns", db.AddOns.ToList());
        }

        [HttpPost]
        public ActionResult DeleteAddon(string AddOnID)
        {
            if (AddOnID == null)
            {
                return Content("Neočekivana greška.", MediaTypeNames.Text.Plain);
            }
            var query = from mealAddOn in db.MealAddOns
                        where mealAddOn.AddOnID.Equals(AddOnID)
                        select mealAddOn;
            var mealAddOns = query.ToList();
            db.MealAddOns.RemoveRange(mealAddOns);

            AddOn addOn = db.AddOns.Find(AddOnID);
            db.AddOns.Attach(addOn);
            db.AddOns.Remove(addOn);
            db.SaveChanges();
            return Content("Dodatak " + addOn.AddOnID + " uspješno obrisan.", MediaTypeNames.Text.Plain);
        }

        ////////////////////////////////////
        //  Add delete Category
        ////////////////////////////////////
        [HttpGet]
        public ActionResult AddCategory()
        {
            return PartialView("AddCategory");
        }

        [HttpPost]
        public ActionResult AddCategory(string categoryName)
        {
            categoryName = categoryName.Trim();
            var exists = db.Categories.FirstOrDefault(s => s.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
            if (exists != null)
            {
                return Content("Kategorija " + categoryName + " već postoji.", MediaTypeNames.Text.Plain);
            }

            db.Categories.Add(new Category { Name = categoryName });
            db.SaveChanges();

            return Content("Kategorija " + categoryName + " uspješno spremljena.", MediaTypeNames.Text.Plain);
        }

        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return PartialView("EditCategoryList", db.Categories.ToList());
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return PartialView("EditCategory", category);
        }

        [HttpPost]
        public ActionResult EditCategory([Bind(Include = "CategoryID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return Content("Kategorija " + category.Name + " uspješno spremljena.", MediaTypeNames.Text.Plain);
            }
            return Content("Kategorija " + category.Name + " nije spremljena.", MediaTypeNames.Text.Plain);
        }

        [HttpGet]
        public ActionResult DeleteCategory()
        {
            return PartialView("DeleteCategory", db.Categories.ToList());
        }

        [HttpPost]
        public void DeleteCategory(int id)
        {
            db.Comments.RemoveRange(db.Comments.Where(type => type.Meal.CategoryID == id));
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.Meal.CategoryID == id));
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.Meal.CategoryID == id));
            db.Meals.RemoveRange(db.Meals.Where(meal => meal.CategoryID == id));
            Category category = new Category { CategoryID = id };
            db.Categories.Attach(category);
            db.Categories.Remove(category);
            db.SaveChanges();
        }

        ////////////////////////////////////
        //  Statistic
        ////////////////////////////////////
        [HttpGet]
        public ActionResult Statistic()
        {
            List<Order> orders = db.Orders.ToList();
            Dictionary<string, int> mealOrders = new Dictionary<string, int>();
            Dictionary<string, int> monthlyOrders = new Dictionary<string, int>();
            Dictionary<string, List<string>> topMealsByMonths = new Dictionary<string, List<string>>();
            Dictionary<string, decimal> monthlyAveragePrices = new Dictionary<string, decimal>();
            Dictionary<string, int> monthyOrderNums = new Dictionary<string, int>();

            int totalNumOfOrders = 0;
            decimal totalAveragePrice = 0.00M;

            foreach (Order order in orders)
            {
                totalNumOfOrders++;
                totalAveragePrice += order.TotalPrice;

                string orderMonth = order.AcceptanceDate.ToString("yyyy-MM");

                if (!monthlyAveragePrices.ContainsKey(orderMonth))
                {
                    monthlyAveragePrices[orderMonth] = order.TotalPrice;
                    monthyOrderNums[orderMonth] = 1;
                }
                else
                {
                    monthlyAveragePrices[orderMonth] += order.TotalPrice;
                    monthyOrderNums[orderMonth] += 1;
                }

                foreach (OrderDetail orderDetail in order.OrderDetails)
                {
                    string mealName = orderDetail.MealName + " - " + orderDetail.MealType;

                    if (!mealOrders.ContainsKey(mealName))
                    {
                        mealOrders[mealName] = 1;
                    }
                    else
                    {
                        mealOrders[mealName] += 1;
                    }
                }

                if (!monthlyOrders.ContainsKey(orderMonth))
                {
                    monthlyOrders[orderMonth] = 1;
                }
                else
                {
                    monthlyOrders[orderMonth] += 1;
                }
            }

            if (totalNumOfOrders != 0)
            {
                totalAveragePrice /= totalNumOfOrders;
            }

            foreach (var amount in monthyOrderNums)
            {
                int num = amount.Value;

                if(num != 0)
                {
                    monthlyAveragePrices[amount.Key] /= num;
                }
            }

            List<KeyValuePair<string, int>> mealList = mealOrders.ToList();

            mealList.Sort((current, next) =>
            {
                return -current.Value.CompareTo(next.Value);
            });

            List<KeyValuePair<string, int>> only3Meals = new List<KeyValuePair<string, int>>();

            int counter = 0;
            foreach (var pair in mealList)
            {
                only3Meals.Add(pair);

                counter++;

                if(counter >= 3)
                {
                    break;
                }
            }

            StatisticsModel model = new StatisticsModel();

            model.TotalAveragePrice = totalAveragePrice;
            model.TotalNumOfOrders = totalNumOfOrders;
            model.TotalTopMeals = only3Meals;
            model.OrdersByMonths = monthlyOrders.ToList();
            model.MonthlyAveragePrices = monthlyAveragePrices;

            return PartialView(model);
        }

        ////////////////////////////////////
        //  Add or delete users
        ////////////////////////////////////

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
                Password = TextUtils.SHA256Hash(pass),
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
            db.Employees.Remove(new Employee() { EmployeeID = employeeId });
            db.SaveChanges();
        }

        ////////////////////////////////////
        //  Static info
        ////////////////////////////////////
        [HttpGet]
        public ActionResult StaticInfo()    //Static info view
        {
            return PartialView("StaticInfoViews/StaticInfo");
        }

        [HttpGet]
        public ActionResult OwnerInfoForm()
        {
            return PartialView("StaticInfoViews/OwnerInfoForm");
        }

        [HttpGet]
        public ActionResult OwnerPictureForm()
        {
            return PartialView("StaticInfoViews/OwnerPictuerUpload");
        }

        [HttpGet]
        public ActionResult RestaurantInfoForm()
        {
            return PartialView("StaticInfoViews/RestaurantInfo");
        }

        [HttpGet]
        public ActionResult RestaurantPictureForm()
        {
            return PartialView("StaticInfoViews/RestauranPictureUpload");
        }

        ////////////////////////////////////
        //  Logout
        ////////////////////////////////////
        public void LogOut()
        {
            SessionExtension.SetUser(Session, null);
        }

        public ActionResult Receipt(Order order)
        {
            return View(order);
        }

        public ActionResult DeleteModal(string Title, string Type)
        {
            return PartialView("DeleteModal", new ModalViewModel() { Title = Title, Type = Type });
        }

        [HttpGet]
        public ActionResult EmployeeMenu()
        {
            return PartialView("Employee/EmployeeMenu");
        }

        [HttpGet]
        public ActionResult EmployeeList()
        {
            List<Employee> list = db.Employees.Where(e => e.isEmployed == true).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
            return PartialView("Employee/EmployeeList", list);
        }

        [HttpGet]
        public ActionResult UnemployedList()
        {
            List<Employee> list = db.Employees.Where(e => e.isEmployed == false).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
            return PartialView("Employee/EmployeeList", list);
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return PartialView("Employee/AddEmployee");
        }

        [HttpPost]
        public ActionResult AddEmployee([Bind(Include = "EmployeeID,Password,FirstName,LastName,Email,PhoneNumber,Address,City,PostCode,Title,isEmployed,AdminRights")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return Content("Djelatnik " + employee.EmployeeID + " uspješno spremljen.", MediaTypeNames.Text.Plain);
                }
            }
            catch (Exception)
            {
                return Content("Korisnicko ime " + employee.EmployeeID + " je vec zauzeto.", MediaTypeNames.Text.Plain);
                throw;
            }
            return Content("Neocekivana greska.", MediaTypeNames.Text.Plain);
        }

        [HttpGet]
        public ActionResult EditEmployee(string EmployeeID)
        {
            if (EmployeeID == null)
            {
                List<Employee> list = db.Employees.Where(e => e.isEmployed == true).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
                return PartialView("Employee/EmployeeList", list);
            }
            Employee employee = db.Employees.Find(EmployeeID);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return PartialView("Employee/EditEmployee", employee);
        }

        [HttpPost]
        public ActionResult EditEmployee([Bind(Include = "EmployeeID,Password,FirstName,LastName,Email,PhoneNumber,Address,City,PostCode,Title,isEmployed,AdminRights")] Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(employee.Password == null || employee.Password.Count() == 0)
                    {
                        db.Employees.Attach(employee);
                        var entry = db.Entry(employee);
                        entry.State = EntityState.Modified;
                        entry.Property(e => e.Password).IsModified = false;
                    }
                    else
                    {
                        employee.Password = TextUtils.SHA256Hash(employee.Password);
                        db.Entry(employee).State = EntityState.Modified;
                    }
                    
                    db.SaveChanges();
                    return Content("Djelatnik " + employee.EmployeeID + " uspješno spremljen.", MediaTypeNames.Text.Plain);
                }
            }
            catch (Exception)
            {
                return Content("Korisnicko ime " + employee.EmployeeID + " je vec zauzeto.", MediaTypeNames.Text.Plain);
                throw;
            }
            return Content("Neocekivana greska.", MediaTypeNames.Text.Plain);
        }

    }
}