using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Models.Cart;

namespace Wild8.DAL
{
    public static class SessionExtensions
    {
        private const string CART_KEY = "CART";
        /// <summary> 
        /// Get value. 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="session"></param> 
        /// <param name="key"></param> 
        /// <returns></returns> 
        public static T GetDataFromSession<T>(this HttpSessionStateBase session, string key)
        {
            return (T)session[key];
        }
        /// <summary> 
        /// Set value. 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="session"></param> 
        /// <param name="key"></param> 
        /// <param name="value"></param> 
        public static void SetDataToSession<T>(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = value;
        }

        public static Cart GetCart(HttpSessionStateBase session)
        {
            Cart Cart = SessionExtensions.GetDataFromSession<Cart>(session, CART_KEY);
            if(Cart == null)
            {
                Cart = new Cart();
                SessionExtensions.SetDataToSession<Cart>(session, CART_KEY, Cart);
            }
            return Cart;
        }
    }
}