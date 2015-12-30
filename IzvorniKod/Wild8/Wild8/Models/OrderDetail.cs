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

        [Key, Column("MealID", Order = 1)]
        public int MealID { get; set; }

        [Key, Column("MealTypeName", Order = 2)]
        public string MealTypeName { get; set; }

        public int Count { get; set; }
        public virtual Order Order { get; set; }
        public virtual MealType MealType { get; set; }

    }
}