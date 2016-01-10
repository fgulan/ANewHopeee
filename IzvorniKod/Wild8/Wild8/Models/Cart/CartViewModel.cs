using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.StaticInfo;

namespace Wild8.Models.Cart
{
    public class CartModelView
    {
        public string StartTime { get; }
        public string EndTime { get; }
        public Cart Cart { get; }

        public CartModelView(Cart cart)
        {
            Cart = cart;
            RestaurauntInfo info = RestaurauntInfo.Instance;
            
            StartTime = info.StarTime;
            EndTime = info.EndTime;
        }
    }
}