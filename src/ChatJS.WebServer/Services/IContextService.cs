using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.WebServer.Services
{
    public interface IContextService
    {
        Task<Guid> CurrentUserAsync();
    }
}
