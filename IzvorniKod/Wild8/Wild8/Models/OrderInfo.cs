using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using Wild8.StaticInfo;

namespace Wild8.Models
{
    public class OrderInfo
    {
        public Order Order { get; set; }
        public RestaurauntInfo Info { get; set; }
        public string Message { get; set; }
    }
}