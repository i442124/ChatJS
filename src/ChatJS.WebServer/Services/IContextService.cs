using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Models;

namespace ChatJS.WebServer.Services
{
    public interface IContextService
    {
        Task<CurrentUserModel> CurrentUserAsync();
    }
}
