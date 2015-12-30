using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class MealType
    {
        [Key, Column("MealID", Order = 0)]
        public int MealID { get; set; }
        [Key, Column("Name", Order = 1)]
        public string MealTypeName { get; set; }
        public virtual Meal Meal { get; set; }
        public decimal Price { get; set; }
    }
}