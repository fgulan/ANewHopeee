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
        [Key, Column("OrderID", Order = 0)]
        public int OrderID { get; set; }
        [Key, Column("MealName", Order = 1)]
        public string MealName { get; set; }
        [Key, Column("MealType", Order = 2)]
        public string MealType { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<OrderMealAddOn> OrderMealAddOns { get; set; }
    }
}