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
        [Key, Column("OrderDetailID", Order = 0)]
        public int OrderDetailID { get; set; }
        [Key, Column("AddOnName", Order = 1)]
        public string AddOnName { get; set; }
        public virtual Order Order { get; set; }
    }
}