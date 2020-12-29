using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Data.Caching
{
    public interface ICacheManager
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire);

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> acquire, TimeSpan experation);

        T GetOrSet<T>(string key, Func<T> acquire);

        T GetOrSet<T>(string key, Func<T> acquire, TimeSpan experation);

        void Remove(string key);
    }
}
