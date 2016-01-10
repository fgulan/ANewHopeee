using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace Wild8.Hubs.Util
{
    public class ConcurentHashSet<T> where T : class
    {
        private HashSet<T> set = new HashSet<T>();

        public void Add(T item)
        {
            lock (set)
            {
                set.Add(item);
            }
        }

        public void Remove(T item)
        {
            lock(set)
            {
                set.Remove(item);
            }
        }

        public IReadOnlyCollection<T> GetSet()
        {
            lock (set)
            {
                return set.ToSafeReadOnlyCollection();
            }
        }
    }
}