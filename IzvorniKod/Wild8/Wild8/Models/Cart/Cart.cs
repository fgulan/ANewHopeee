using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;

namespace Wild8.Models.Cart
{
    public class Cart
    {
        private List<CartItem> items;

        public decimal TotalPrice
        {
            get
            {
                decimal sum = 0;
                foreach (var cartItem in items)
                {
                    sum += cartItem.Price;
                }
                return sum;
            }
        }

        public Cart()
        {
            items = new List<CartItem>();
        }

        public void AddItem(CartItem Item)
        {
            items.Add(Item);
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
            items.Remove(Item);
        }

        public decimal ChangeItemCount(CartItem item, int newCount)
        {
            CartItem cartItem =items.Find(i => i.Equals(item));
            if (cartItem != null)
            {
                cartItem.Count = newCount;
            }
            else
            {
                return 0;
            }
            return cartItem.Price;
        }

        public int Count()
        {
            return items.Count;
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool IsEmpty()
        {
            return items.Count == 0;
        }
    }
}