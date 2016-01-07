using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class AddOn
    {
        [Key]
        public string AddOnID { get; set; }
        public decimal Price { get; set; }
        public bool isActive { get; set; }
    }
}