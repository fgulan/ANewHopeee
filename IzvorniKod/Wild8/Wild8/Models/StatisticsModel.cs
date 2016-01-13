using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Models
{
    public class StatisticsModel
    {
        public int TotalNumOfOrders { get; set; }
        public decimal TotalAveragePrice { get; set; }
        // string -> meal name, int -> num of orders
        public List<KeyValuePair<string, int>> TotalTopMeals { get; set; }
        // string -> date, int -> num of orders
        public List<KeyValuePair<string, int>> OrdersByMonths { get; set; }
        // string -> date, string -> meal name + meal type + num of orders
        public Dictionary<string, List<KeyValuePair<string, int>>> TopMealsByMonths { get; set; }
        // string -> date, decimal -> average price
        public Dictionary<string, decimal> MonthlyAveragePrices { get; set; }
    }
}