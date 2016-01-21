using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNet.SignalR;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Hubs;
using Wild8.Hubs.Util;
using Wild8.Models;
using Wild8.Models.ModelViews;
using Wild8.StaticInfo;
using Wild8.Utils;

namespace Wild8.Controllers
{
    public class AdminController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();
        private CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

        private bool LoggedOut()
        {
            if (SessionExtension.GetUser(Session) == null)
            {
                return true;
            }
            return false;
        }

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

        [HttpPost]
        public ActionResult Order(string JsonOrder)
        {
            var order = JsonConvert.DeserializeObject<Order>(JsonOrder);
            order.OrderDate = DateTime.Now;
            JsonOrder = JsonConvert.SerializeObject(order);

            OrdersSet.GetInstance().Add(JsonOrder); //Send order to workers
            var context = GlobalHost.ConnectionManager.GetHubContext<OrderHub>();
            context.Clients.Group(OrderHub.WORKERS).addNewOrder(JsonOrder);

            SessionExtension.GetCart(Session).Clear();  //Clear cart
            return PartialView("~/Views/Cart/ThankYouMsg.cshtml");
        }

        ////////////////////////////////////
        //  Meal add, edit, delete
        ////////////////////////////////////
        [HttpPost]
        public ActionResult AcceptOrder(string orderJSON, string employeeId, string message)
        {
            if (LoggedOut()) return HttpNotFound();
            var acceptedOrder = JsonConvert.DeserializeObject<Order>(orderJSON);
            acceptedOrder.EmpolyeeID = employeeId;
            acceptedOrder.AcceptanceDate = DateTime.Now;

            OrderInfo info = new OrderInfo()
            {
                Info = RestaurauntInfo.Instance,
                Message = message,
                Order = acceptedOrder
            };

            updateMealOrderCount(acceptedOrder);

            using (StringWriter sw = new StringWriter())
            {
                ControllerContext.Controller.ViewData.Model = info;
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, "Receipt");
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                var receipt = sw.ToString();

                MailUtil.Instance.SendReceiptTo(acceptedOrder.Email, "Potvrda narudžbe", receipt);
            }

            db.Orders.Add(acceptedOrder);
            db.SaveChanges();
            return Content("Narudzba potvrdjena");
        }

        private void updateMealOrderCount(Order acceptedOrder)
        {
            //I know this is affull but do not know better method
            foreach (var orderDetail in acceptedOrder.OrderDetails)
            {
                var meal = db.Meals.FirstOrDefault(m => m.Name == orderDetail.MealName);
                if (meal != null) meal.NumberOfOrders += orderDetail.Count;
            }
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult RefuseOrder(string email, string message)
        {
            if (LoggedOut()) return HttpNotFound();
            MailUtil.Instance.SendReceiptTo(email, "Naruđba odbijena", message);
            return Content("Narudzba odbijena");
        }

        [HttpGet]
        public ActionResult AddEditDelMenu()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("AddEditDelMealsPartial");
        }

        [HttpGet]
        public ActionResult AddMeal()
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    meal.ImagePath = UploadPhoto(upload);
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

        private string UploadPhoto(HttpPostedFileBase photo)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetFileName(photo.FileName);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("meals");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(photo.InputStream);
            return blockBlob.StorageUri.PrimaryUri.AbsoluteUri;
        }

        private void DeletePhoto(string path)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("meals");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            try
            {
                CloudBlockBlob blockBlob = new CloudBlockBlob(new Uri(path), storageAccount.Credentials);
                blockBlob.DeleteIfExists();
            }
            catch (Exception)
            {
            }
        }

        [HttpGet]
        public ActionResult EditMeal(int? id)
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    DeletePhoto(meal.ImagePath);
                    meal.ImagePath = UploadPhoto(upload);
                }

                db.Meals.Attach(meal);
                var entry = db.Entry(meal);
                entry.State = EntityState.Modified;
                entry.Property(e => e.NumberOfOrders).IsModified = false;
                entry.Property(e => e.Grade).IsModified = false;

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
            if (LoggedOut()) return HttpNotFound();
            return PartialView("DelMeals", db.Meals.ToList());
        }

        [HttpPost]
        public ActionResult DeleteMeal(int mealId)
        {   //Performance wise is awful but sintax is nice
            //To make it better use sql command
            if (LoggedOut()) return HttpNotFound();
            db.Comments.RemoveRange(db.Comments.Where(type => type.MealID == mealId));
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.MealID == mealId));
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.MealID == mealId));
            var meal = new Meal { MealID = mealId };
            db.Meals.Attach(meal);
            db.Meals.Remove(meal);
            db.SaveChanges();
            return Content("Jelo obrisano");
        }

        ////////////////////////////////////
        //  Add delete addon
        ////////////////////////////////////
        public ActionResult AddAddOn()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("AddAddOn");
        }

        [HttpPost]
        public ActionResult AddAddOn(string Name, string Price)
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            return PartialView("DelAddOns", db.AddOns.ToList());
        }

        [HttpPost]
        public ActionResult DeleteAddon(string AddOnID)
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            return PartialView("AddCategory");
        }

        [HttpPost]
        public ActionResult AddCategory(string categoryName)
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            return PartialView("DeleteCategory", db.Categories.ToList());
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (LoggedOut()) return HttpNotFound();
            db.Comments.RemoveRange(db.Comments.Where(type => type.Meal.CategoryID == id));
            db.MealTypes.RemoveRange(db.MealTypes.Where(type => type.Meal.CategoryID == id));
            db.MealAddOns.RemoveRange(db.MealAddOns.Where(mealAddOn => mealAddOn.Meal.CategoryID == id));
            db.Meals.RemoveRange(db.Meals.Where(meal => meal.CategoryID == id));
            Category category = new Category { CategoryID = id };
            db.Categories.Attach(category);
            db.Categories.Remove(category);
            db.SaveChanges();
            return Content("Kategorija obrisana");
        }

        ////////////////////////////////////
        //  Statistic File
        ////////////////////////////////////
        [HttpGet]
        public void Report(string reportType)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] contentAsBytes = encoding.GetBytes(GenerateReport());

            this.HttpContext.Response.ContentType = "application/octet-stream";
            this.HttpContext.Response.AddHeader("Content-Disposition", "filename=" + "statistika.txt");
            this.HttpContext.Response.Buffer = true;
            this.HttpContext.Response.Clear();
            this.HttpContext.Response.OutputStream.Write(contentAsBytes, 0, contentAsBytes.Length);
            this.HttpContext.Response.OutputStream.Flush();
            this.HttpContext.Response.End();
        }

        [HttpGet]
        public void DownloadOrder(string username, string orderDate)
        {
            string strOrder = null;
            foreach(var ord in OrdersSet.GetInstance().GetSet())
            {
                if(ord.Contains(username))
                {
                    Order parsedOrder = JsonConvert.DeserializeObject<Order>(ord);
                    DateTime orderTime = DateTime.Parse(orderDate);

                    if(Equals(orderTime, parsedOrder.OrderDate))
                    {
                        strOrder = ord;
                        break;
                    }
                }
            }

            if (strOrder == null) return;

            Order order = JsonConvert.DeserializeObject<Order>(strOrder);

            StringBuilder sb = new StringBuilder();
            sb.Append("Klijent: ").Append(order.Name).AppendLine()
              .Append("Email: ").Append(order.Email).AppendLine()
              .Append("Adresa: ").Append(order.Address).AppendLine()
              .Append("Datum narudžbe: ").Append(order.OrderDate).AppendLine()
              .Append("Ukupna cijena ").Append(order.TotalPrice).AppendLine();

            foreach(var orderDetail in order.OrderDetails)
            {
                sb.Append("####################################").AppendLine()
                    .Append("\tJelo: ").Append(orderDetail.MealName).AppendLine()
                    .Append("\tVeličina: ").Append(orderDetail.MealType).AppendLine()
                    .Append("\tKoličina: ").Append(orderDetail.Count).AppendLine()
                    .Append("\tDodaci: ").AppendLine();
                foreach (var addOn in orderDetail.OrderMealAddOns)
                {
                    sb.Append("\t\t + ").Append(addOn.AddOnName).AppendLine();
                }
            }

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] contentAsBytes = encoding.GetBytes(sb.ToString());

            this.HttpContext.Response.ContentType = "application/octet-stream";
            this.HttpContext.Response.AddHeader("Content-Disposition", "filename=" + "narudrba.txt");
            this.HttpContext.Response.Buffer = true;
            this.HttpContext.Response.Clear();
            this.HttpContext.Response.OutputStream.Write(contentAsBytes, 0, contentAsBytes.Length);
            this.HttpContext.Response.OutputStream.Flush();
            this.HttpContext.Response.End();
        }

        private string GenerateReport()
        {
            List<Order> orders = db.Orders.ToList();

            orders.Sort((current, next) =>
            {
                return current.AcceptanceDate.CompareTo(next.AcceptanceDate);
            });

            StringBuilder builder = new StringBuilder();

            foreach (var order in orders)
            {
                builder.Append(order.AcceptanceDate);
                builder.Append(", adresa: ");
                builder.Append(order.Address);
                builder.Append(", e-mail: ");
                builder.Append(order.Email);
                builder.Append(", ukupna cijena: ");
                builder.Append(order.TotalPrice);
                builder.Append(" kn, zaposlenik: ");
                builder.Append(order.EmpolyeeID);
                builder.AppendLine();
            }

            return builder.ToString();
        }

        ////////////////////////////////////
        //  Statistic
        ////////////////////////////////////
        [HttpGet]
        public ActionResult Statistic()
        {
            if (LoggedOut()) return HttpNotFound();
            List<Order> orders = db.Orders.ToList();
            Dictionary<string, int> mealOrders = new Dictionary<string, int>();
            Dictionary<string, int> monthlyOrders = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, int>> topMealsByMonths = new Dictionary<string, Dictionary<string, int>>();
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

                    // monthly meals
                    Dictionary<string, int> currentMonthTopMeals = null;

                    // get correct dictionary
                    if (!topMealsByMonths.ContainsKey(orderMonth))
                    {
                        currentMonthTopMeals = new Dictionary<string, int>();
                        topMealsByMonths[orderMonth] = currentMonthTopMeals;
                    }
                    else
                    {
                        currentMonthTopMeals = topMealsByMonths[orderMonth];
                    }

                    // refresh meal counter in that dictionary
                    if (!currentMonthTopMeals.ContainsKey(mealName))
                    {
                        currentMonthTopMeals[mealName] = 1;
                    }
                    else
                    {
                        currentMonthTopMeals[mealName] += 1;
                    }
                    // end
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

                if (num != 0)
                {
                    monthlyAveragePrices[amount.Key] /= num;
                }
            }

            // get total top meals
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

                if (counter >= 3)
                {
                    break;
                }
            }

            // get top meals for every month
            Dictionary<string, List<KeyValuePair<string, int>>> topMealsPerMonthList = new Dictionary<string, List<KeyValuePair<string, int>>>();

            foreach (var pair in topMealsByMonths)
            {
                List<KeyValuePair<string, int>> monthMealList = pair.Value.ToList();

                monthMealList.Sort((current, next) =>
                {
                    return -current.Value.CompareTo(next.Value);
                });

                List<KeyValuePair<string, int>> monthOnly3Meals = new List<KeyValuePair<string, int>>();

                counter = 0;
                foreach (var monthPair in monthMealList)
                {
                    monthOnly3Meals.Add(monthPair);

                    counter++;

                    if (counter >= 3)
                    {
                        break;
                    }
                }

                topMealsPerMonthList[pair.Key] = monthMealList;
            }

            StatisticsModel model = new StatisticsModel();

            model.TotalAveragePrice = totalAveragePrice;
            model.TotalNumOfOrders = totalNumOfOrders;
            model.TotalTopMeals = only3Meals;
            model.OrdersByMonths = monthlyOrders.ToList();
            model.MonthlyAveragePrices = monthlyAveragePrices;
            model.TopMealsByMonths = topMealsPerMonthList;

            return PartialView(model);
        }

        ////////////////////////////////////
        //  Add or delete users
        ////////////////////////////////////

        public ActionResult AddEmployee(string employeeId, string pass,
                            string firstName, string lastName,
                            string email, string phoneNumber,
                            string address, string city, string postCode,
                            string title)
        {
            if (LoggedOut()) return HttpNotFound();
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
            return Content("Djelatnik dodan");
        }

        public ActionResult RemoveEmployee(string employeeId)
        {
            if (LoggedOut()) return HttpNotFound();
            db.Employees.Remove(new Employee() { EmployeeID = employeeId });
            db.SaveChanges();
            return Content("Djelatnik obrisan");
        }

        ////////////////////////////////////
        //  Static info
        ////////////////////////////////////
        [HttpGet]
        public ActionResult StaticInfo()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("StaticInfoViews/StaticInfo", RestaurauntInfo.Instance);
        }

        [HttpGet]
        public ActionResult OwnerInfoForm()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("StaticInfoViews/OwnerInfoForm", RestaurauntInfo.Instance);
        }

        [HttpGet]
        public ActionResult RestaurantInfoForm()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("StaticInfoViews/RestaurantInfo", RestaurauntInfo.Instance);
        }

        ////////////////////////////////////
        //  Logout
        ////////////////////////////////////
        public void LogOut()
        {
            SessionExtension.SetUser(Session, null);
        }

        public ActionResult DeleteModal(string Title, string Type)
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("DeleteModal", new ModalViewModel() { Title = Title, Type = Type });
        }

        [HttpGet]
        public ActionResult EmployeeMenu()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("Employee/EmployeeMenu");
        }

        [HttpGet]
        public ActionResult EmployeeList()
        {
            if (LoggedOut()) return HttpNotFound();
            List<Employee> list = db.Employees.Where(e => e.isEmployed == true).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
            return PartialView("Employee/EmployeeList", list);
        }

        [HttpGet]
        public ActionResult UnemployedList()
        {
            if (LoggedOut()) return HttpNotFound();
            List<Employee> list = db.Employees.Where(e => e.isEmployed == false).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToList();
            return PartialView("Employee/EmployeeList", list);
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            if (LoggedOut()) return HttpNotFound();
            return PartialView("Employee/AddEmployee");
        }

        [HttpPost]
        public ActionResult AddEmployee([Bind(Include = "EmployeeID,Password,FirstName,LastName,Email,PhoneNumber,Address,City,PostCode,Title,isEmployed,AdminRights")] Employee employee)
        {
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
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
            if (LoggedOut()) return HttpNotFound();
            try
            {
                if (ModelState.IsValid)
                {
                    if (employee.Password == null || employee.Password.Count() == 0)
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

        [HttpPost]
        public ActionResult EditRestaurauntInfo(string OwnerAddress, string OwnerCity, string OwnerPhone, string RestaurantEmail, string RestStartH,
                                                string RestauranutHomepageInfo, string RestStartM, string RestEndH, string RestEndM, string MinimalOrderPrice)
        {
            if (LoggedOut()) return HttpNotFound();
            RestaurauntInfo info = RestaurauntInfo.Instance;
            info.RestauranutHomepageInfo = RestauranutHomepageInfo;
            info.OwnerAddress = OwnerAddress;
            info.OwnerCity = OwnerCity;
            info.OwnerPhone = OwnerPhone;
            info.RestaurantEmail = RestaurantEmail;
            info.RestStartH = RestStartH;
            info.RestStartM = RestStartM;
            info.RestEndH = RestEndH;
            info.RestEndM = RestEndM;
            info.MinimalOrderPrice = Decimal.Parse(MinimalOrderPrice.Replace('.', ','));
            info.SaveData();
            return Content("Informacije uspješno spremljene.", MediaTypeNames.Text.Plain);
        }

        [HttpPost]
        public ActionResult EditOwnerInfo(string OwnerEMail, string OwnerName, string OwnerHomepageInfo, string OwnerContactInfo)
        {
            if (LoggedOut()) return HttpNotFound();
            RestaurauntInfo info = RestaurauntInfo.Instance;
            info.OwnerEMail = OwnerEMail;
            info.OwnerName = OwnerName;
            info.OwnerHomepageInfo = OwnerHomepageInfo;
            info.OwnerContactInfo = OwnerContactInfo;
            info.SaveData();
            return Content("Informacije uspješno spremljene.", MediaTypeNames.Text.Plain);
        }
    }
}