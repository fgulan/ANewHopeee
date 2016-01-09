using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Wild8.DAL;
using Wild8.Models;
using Wild8.Models.Cart;

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

        private bool CreateOrder(string Name, string Address, string Phone, string Email, string Note)
        {
            Cart cart = SessionExtension.GetCart(Session);

            Order order = new Order();

            order.Name = Name;

            string[] decomposedAddress = Regex.Split(Address, "(, )|(,)"); //Ovo nije bilo namjerno
            if (decomposedAddress.Length != 3) { return false; }
            order.Address = decomposedAddress[0];
            order.City = decomposedAddress[1];
            order.PostCode = decomposedAddress[2];

            order.Annotation = Note;

            order.OrderDate = DateTime.Now;

            order.TotalPrice = cart.TotalPrice;

            foreach (CartItem item in cart.Items)
            {
                OrderDetail orderDetail = new OrderDetail();

                order.OrderID = order.OrderID;
                orderDetail.MealName = item.MealType.Meal.Name;
                orderDetail.MealType = item.MealType.ToString();
                orderDetail.Price = item.Price;
                orderDetail.Count = item.Count;
                orderDetail.Order = order;

                foreach (AddOn addOn in item.AddOns)
                {
                    OrderMealAddOn newAddOn = new OrderMealAddOn();

                    newAddOn.OrderID = order.OrderID;
                    //newAddOn.MealName = item.MealType.Meal.Name;
                    //TODO: meal name je int??
                    newAddOn.AddOnName = addOn.AddOnID;
                    newAddOn.Price = addOn.Price;
                    newAddOn.Order = order;

                    orderDetail.OrderMealAddOns.Add(newAddOn);
                }

                order.OrderDetails.Add(orderDetail);
            }

            return true;
        }
    }
}