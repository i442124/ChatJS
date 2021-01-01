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
        private readonly IContextService _contextService;
        private readonly INotificationService _notificationService;

        public ChatHub(
            IContextService contextService,
            INotificationService notificationService)
        {
            _contextService = contextService;
            _notificationService = notificationService;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await _notificationService.SignInAsync(
                Context.ConnectionId, (await _contextService.CurrentUserAsync()).Id);
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            await _notificationService.SignOffAsync(Context.ConnectionId);
        }
    }
}
