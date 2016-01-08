using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models.Cart
{
    public class Cart
    {
        private List<CartItem> items;

        private decimal totalPrice;

        public decimal TotalPrice
        {
            get
            {
                return totalPrice;
            }
        }

        public Cart()
        {
            items = new List<CartItem>();
            totalPrice = 0;
        }

        public void AddItem(CartItem Item)
        {
            items.Add(Item);
            totalPrice += Item.Price;
        }

        public IReadOnlyList<CartItem> Items
        {
            get
            {
                return items.AsReadOnly();
            }
        }

        public void RemoveItem(CartItem Item)
        {
            if (items.Remove(Item))
            {
                totalPrice -= Item.Price;
            }
        }

        public int Count()
        {
            return items.Count;
        }

        public bool IsEmpty()
        {
            return items.Count == 0;
        }
    }
}