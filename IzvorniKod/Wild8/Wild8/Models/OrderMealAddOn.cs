using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class OrderMealAddOn
    {
        [Key, Column("OrderID", Order = 0)]
        public int OrderID { get; set; }
        [Key, Column("MealName", Order = 1)]
        public int MealName { get; set; }
        [Key, Column("AddOnName", Order = 2)]
        public string AddOnName { get; set; }
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
    }
}