using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class MealType : IComparable<MealType>
    {
        [Key, Column("MealID", Order = 0)]
        public int MealID { get; set; }

        [Key, Column("MealTypeName", Order = 1)]
        public string MealTypeName { get; set; }

        [JsonIgnore]
        public virtual Meal Meal { get; set; }

        public decimal Price { get; set; }

        protected bool Equals(MealType other)
        {
            return MealID == other.MealID && string.Equals(MealTypeName, other.MealTypeName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MealType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (MealID * 397) ^ (MealTypeName != null ? MealTypeName.GetHashCode() : 0);
            }
        }

        int IComparable<MealType>.CompareTo(MealType other)
        {
            return MealTypeName.CompareTo(other.MealTypeName);
        }
    }
}