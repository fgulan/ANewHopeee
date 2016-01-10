using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public string MealName { get; set; }
        public string MealType { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<OrderMealAddOn> OrderMealAddOns { get; set; }
    }
}