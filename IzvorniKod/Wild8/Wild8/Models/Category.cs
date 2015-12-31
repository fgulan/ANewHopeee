using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}