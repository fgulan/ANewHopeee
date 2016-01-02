using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wild8.Models.Cart;
using Wild8.DAL;
using Wild8.Models;

namespace Wild8.Controllers
{
    public class CartController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public int AddMeal(int MealID, string Type, int Count, string[] AddOns)
        {
            Cart cart = SessionExtension.GetCart(Session);
            string name = Type.Split('#')[1];
            MealType MealType = db.MealTypes.Find(MealID, name);
            CartItem CartItem = new CartItem(MealType, Count);

            foreach (var item in AddOns)
            {
                var ID = item.Split('#')[1];
                AddOn addOn = db.AddOns.Find(ID);
                CartItem.AddMealAddOn(addOn);
            }

            cart.AddItem(CartItem);
            int count =cart.Count();
            Session["CartCount"] = count;
            return count;
        }
    }
}