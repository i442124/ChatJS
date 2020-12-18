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
        private readonly IBroadcastService _service;

        public ChatHub(IBroadcastService service)
        {
            _service = service;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            //await _service.RegisterAsync(Context.ConnectionId);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            //await _service.UnregisterAsync(Context.ConnectionId);
        }
    }
}
