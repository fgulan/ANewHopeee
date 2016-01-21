using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public int Grade { get; set; }
        public DateTime CommentDate { get; set; }
        public int? MealID { get; set; }
        public virtual Meal Meal { get; set; }
    }
}