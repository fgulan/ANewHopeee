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
        public List<KeyValuePair<string, int>> TotalTopMeals { get; set; }
        public List<KeyValuePair<string, int>> OrdersByMonths { get; set; }
    }
}