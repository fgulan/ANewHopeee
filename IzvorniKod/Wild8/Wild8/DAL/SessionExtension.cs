﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wild8.Models;
using Wild8.Models.Cart;

namespace Wild8.DAL
{
    public static class SessionExtension
    {
        private const string CART_KEY = "CART";
        private const string CART_COUNT = "CartCount";

        private const string USER_KEY = "USER";

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
            Cart Cart = SessionExtension.GetDataFromSession<Cart>(session, CART_KEY);
            if (Cart == null)
            {
                Cart = new Cart();
                SessionExtension.SetDataToSession<Cart>(session, CART_KEY, Cart);
            }
            return Cart;
        }

        public static void SetCartItemCount(int count, HttpSessionStateBase session)
        {
            session[CART_COUNT] = count;
        }

        public static void SetUser(HttpSessionStateBase session, Employee employee)
        {
            session[USER_KEY] = employee;
        }

        public static Employee GetUser(HttpSessionStateBase session)
        {
            return (Employee)session[USER_KEY];
        }

        public static void LogoutUser(HttpSessionStateBase session)
        {
            session[USER_KEY] = null;
        }
    }
}