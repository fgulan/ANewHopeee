using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.ModelViews;
using Wild8.Models.Cart;

namespace Wild8.Controllers
{
    public class MealsController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();
        // GET: Meals

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var meal = db.Meals.Find(id);
            if (meal == null)
            {
                return HttpNotFound();
            }
            if (!meal.IsAvailable)
            {
                return HttpNotFound();
            }
            MealWithPrice mwp = new MealWithPrice()
            {
                Meal = meal,
                Types = db.MealTypes.Where(type => type.MealID == meal.MealID).ToList(),
                IsHot = false
            };

            return View(mwp);
        }

        // POST: Meals/AddNewComment
        // TODO shity stars
        [HttpPost]
        public ActionResult AddNewComment(int? ID, string Username, string Message, int? Rating)
        {
            if (ModelState.IsValid)
            {
                Comment newComment = new Comment
                {
                    CommentDate = DateTime.Now,
                    Message = Message,
                    Username = Username,
                    Grade = Rating == null ? 5 : (int)Rating,
                    MealID = ID
                };
                db.Comments.Add(newComment);
                db.SaveChanges();

                return PartialView("~/Views/Meals/Comment.cshtml", newComment);
            }

            return null;
        }

        //POST
        public int AddToCart(int count, int mealID, string mealTypeName, string[] addOnNames) 
        {
            MealType type = db.MealTypes.Find(mealID, mealTypeName);
            CartItem item = new CartItem(type, count);

            if (addOnNames != null)
            {
                foreach (var addOnName in addOnNames)
                {
                    item.AddMealAddOn(db.AddOns.Find(addOnName));
                }
            }

            Cart cart = SessionExtension.GetCart(Session);
            cart.AddItem(item);
            SessionExtension.SetCartItemCount(cart.Count(), Session);
            return cart.Count();
        }
    }
}
