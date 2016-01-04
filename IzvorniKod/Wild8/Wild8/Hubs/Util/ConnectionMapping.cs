using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wild8.Hubs.Util
{
    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, string> _connections =
            new Dictionary<T, string>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out connectionId))
                {
                    _connections.Add(key, connectionId);
                }
            }
        }

        public string GetConnection(T key)
        {
            string connection;
            if (_connections.TryGetValue(key, out connection))
            {
                return connection;
            }

            return null;
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out connectionId))
                {
                    return;
                }

                _connections.Remove(key);
            }
        }
    }
}