using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace ChatJS.Data.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetOrSet<T>(string key, Func<T> acquire)
        {
            return GetOrSet(key, acquire, TimeSpan.FromSeconds(30));
        }

        public T GetOrSet<T>(string key, Func<T> acquire, TimeSpan experation)
        {
            if (_memoryCache.TryGetValue(key, out T data))
            {
                return data;
            }

            data = acquire();
            _memoryCache.Set(key, data, experation);

            return data;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire)
        {
            return await GetOrSetAsync(key, acquire, experation: TimeSpan.FromSeconds(30));
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire, TimeSpan experation)
        {
            if (_memoryCache.TryGetValue(key, out T data))
            {
                return data;
            }

            data = await acquire();
            _memoryCache.Set(key, data, experation);

            return data;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
