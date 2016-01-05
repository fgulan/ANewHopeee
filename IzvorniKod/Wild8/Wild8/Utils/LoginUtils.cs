using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Wild8.Utils
{
    public class LoginUtils
    {
        private static Regex emailPattern = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

        public static String SHA256Hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }

        public static bool IsEmail(String value)
        {
            return emailPattern.IsMatch(value);
        }

        public static String sanitize(String value)
        {
            // TODO: escape for SQL injection.
            return value;
        }
    }
}