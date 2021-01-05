using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatJS.WebServer.Hubs
{
    public interface IHubConnectionMapper<T> : IEnumerable<T>
    {
        void Remove(string connectionId);

        Task RemoveAsync(string connectionId);

        void Add(string connectionId, T value);

        Task AddAsync(string connectionId, T value);

        T GetConnectionInfo(string connectionId);

        Task<T> GetConnectionInfoAsync(string connectionId);

        IEnumerable<string> GetConnectionIds(T value);

        Task<IEnumerable<string>> GetConnectionIdsAsync(T value);
    }
}
