using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class AddOn
    {
        [Key]
        public string AddOnID { get; set; }
        public decimal Price { get; set; }

        protected bool Equals(AddOn other)
        {
            return string.Equals(AddOnID, other.AddOnID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AddOn) obj);
        }

        public override int GetHashCode()
        {
            return (AddOnID != null ? AddOnID.GetHashCode() : 0);
        }
    }
}