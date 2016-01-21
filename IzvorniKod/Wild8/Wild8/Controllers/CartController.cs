using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.Cart;
using Wild8.Utils;

namespace Wild8.Controllers
{
    public class CartController : Controller
    {
        private RestaurauntContext db = new RestaurauntContext();

        // GET: Cart
        public ActionResult Index()
        {
            return View(new CartModelView(SessionExtension.GetCart(Session)));
        }

        [HttpPost]
        public int RemoveMeal(int MealID, string Type, int Count, string[] AddOns)
        {
            CartItem item = createCartItem(MealID, Type, Count, AddOns);
            Cart cart = SessionExtension.GetCart(Session);
            cart.RemoveItem(item);
            int count = cart.Count();
            Session["CartCount"] = count;
            return count;
        }

        public decimal ChangeCartItemSize(int MealID, string Type, int oldCount, string[] AddOns, int newCount)
        {
            CartItem item = createCartItem(MealID, Type, oldCount, AddOns);
            Cart cart = SessionExtension.GetCart(Session);
            return cart.ChangeItemCount(item, newCount);
        }

        private CartItem createCartItem(int MealID, string Type, int Count, string[] AddOns)
        {
            Cart cart = SessionExtension.GetCart(Session);
            MealType MealType = db.MealTypes.Find(MealID, Type);
            CartItem CartItem = new CartItem(MealType, Count);

            if (AddOns != null)
            {
                foreach (var item in AddOns)
                {
                    AddOn addOn = db.AddOns.Find(item);
                    CartItem.AddMealAddOn(addOn);
                }
            }
            return CartItem;
        }
    }
}