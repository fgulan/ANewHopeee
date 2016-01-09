using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Wild8.Models.AdminModels
{
    public class OrderModelView
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UserNote { get; set; }
        public List<OrderMealModelView> Meals { get; set; }
    }

    public class OrderMealModelView
    {
        public string MealName { get; set; }
        public string MealTypeName { get; set; }
        public int Count { get; set; }
        public List<string> Addons { get; set; }
    }
    

}