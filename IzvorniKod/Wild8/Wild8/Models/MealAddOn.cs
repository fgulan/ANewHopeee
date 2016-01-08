using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class MealAddOn
    {
        [Key, Column("MealID", Order = 0)]
        public int MealID { get; set; }
        [Key, Column("AddOnID", Order = 1)]
        public string AddOnID { get; set; }
        [JsonIgnore]
        public virtual Meal Meal { get; set; }
        public virtual AddOn AddOn { get; set; }


    }
}