using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.WebServer.Hubs
{
    public class HubConnectionMapper<T> : IHubConnectionMapper<T>
    {
        private readonly ConcurrentDictionary<string, T> _connections =
        new ConcurrentDictionary<string, T>();

        public int Count
        {
            get { return _connections.Count; }
        }

        public void Add(string connectionId, T value)
        {
            _connections[connectionId] = value;
        }

        public Task AddAsync(string connectionId, T value)
        {
            return Task.Run(() => Add(connectionId, value));
        }

        public void Remove(string connectionId)
        {
            if (!_connections.TryRemove(connectionId, out _))
            {
                throw new ArgumentException("Connection does not exists.");
            }
        }

        public Task RemoveAsync(string connectionId)
        {
            return Task.Run(() => Remove(connectionId));
        }

        public T GetConnectionInfo(string connectionId)
        {
            if (!_connections.TryGetValue(connectionId, out T value))
            {
                throw new ArgumentException("Connection does not exists.");
            }

            return value;
        }

        public Task<T> GetConnectionInfoAsync(string connectionId)
        {
            return Task.Run(() => GetConnectionInfo(connectionId));
        }

        public IEnumerable<string> GetConnectionIds(T value)
        {
            return _connections
                .Where(pair => pair.Value.Equals(value))
                .Select(pair => pair.Key);
        }

        public Task<IEnumerable<string>> GetConnectionIdsAsync(T value)
        {
            return Task.FromResult(GetConnectionIds(value));
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var userStore in _connections.Values)
            {
                yield return userStore;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var userStore in _connections.Values)
            {
                yield return userStore;
            }
        }
    }
}
