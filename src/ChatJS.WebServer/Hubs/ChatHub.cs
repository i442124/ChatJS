using System;
using System.Threading.Tasks;

using ChatJS.WebServer;
using ChatJS.WebServer.Services;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.SignalR;

namespace ChatJS.WebServer.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
