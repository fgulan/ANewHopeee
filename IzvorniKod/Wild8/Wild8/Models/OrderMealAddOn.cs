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
        [Key, Column("MealID", Order = 1)]
        public int MealID { get; set; }
        [Key, Column("AddOnID", Order = 2)]
        public string AddOnID { get; set; }
        public Order Order { get; set; }
        public Meal Meal { get; set; }
        public AddOn AddOn { get; set; }
    }
}