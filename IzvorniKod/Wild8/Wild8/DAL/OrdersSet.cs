using System.Collections.Generic;
using WebGrease.Css.Extensions;

namespace Wild8.Hubs.Util
{
    public class OrdersSet 
    {
        private static OrdersSet instance; 

        private OrdersSet() { } 
        
        private HashSet<string> set = new HashSet<string>();

        public static OrdersSet GetInstance()
        {
            if (instance == null)
            {
                instance = new OrdersSet();
            }
            return instance;
        }

        public void Add(string item)
        {
            lock (set)
            {
                set.Add(item);
            }
        }

        public bool Remove(string item)
        {
            lock(set)
            {
                return set.Remove(item);
            }
        }

        public IReadOnlyCollection<string> GetSet()
        {
            lock (set)
            {
                return set.ToSafeReadOnlyCollection();
            }
        }
    }
}