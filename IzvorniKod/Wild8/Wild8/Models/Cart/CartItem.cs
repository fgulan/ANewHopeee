using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.Cart
{
    public class CartItem
    {
        private MealType mealType;
        private List<AddOn> addOns;
        private int count;
        private decimal totalPrice;

        public CartItem(MealType MealType, int Count)
        {
            mealType = MealType;
            addOns = new List<AddOn>();
            count = Count;
            totalPrice = count * mealType.Price;
        }

        public void AddMealAddOn(AddOn AddOn)
        {
            addOns.Add(AddOn);
            totalPrice += count * AddOn.Price;
        }

        public void RemoveMealAddOn(AddOn AddOn)
        {
            if (addOns.Remove(AddOn))
            {
                totalPrice -= count * AddOn.Price;
            }
        }
        public decimal Price
        {
            get
            {
                return totalPrice;
            }
        }
    }
}