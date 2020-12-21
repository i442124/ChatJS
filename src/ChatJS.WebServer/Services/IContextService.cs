using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.WebServer;
using ChatJS.WebServer.Models;

namespace ChatJS.WebServer.Services
{
    public interface IContextService
    {
        Task<CurrentUserModel> CurrentUserAsync();
    }
}
