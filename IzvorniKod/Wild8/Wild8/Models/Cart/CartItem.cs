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

        public MealType MealType
        {
            get
            {
                return mealType;
            }
        }

        public IReadOnlyList<AddOn> AddOns
        {
            get
            {
                return addOns.AsReadOnly();
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                totalPrice = mealType.Price * count;
                foreach (var addOn in AddOns)
                {
                    totalPrice += addOn.Price;
                }
            }
        }

        protected bool Equals(CartItem other)
        {
            bool b1 = Equals(mealType, other.mealType);
            var  b2 = !addOns.Except(other.addOns).Union(addOns.Except(other.addOns)).Any();
            bool b3 = count == other.count;
            return b1 && b2 && b3;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CartItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (mealType != null ? mealType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (addOns != null ? addOns.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ count;
                return hashCode;
            }
        }
    }
}