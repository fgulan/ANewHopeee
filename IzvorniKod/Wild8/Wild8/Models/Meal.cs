using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class Meal
    {
        public int MealID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<MealAddOn> AddOns { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}