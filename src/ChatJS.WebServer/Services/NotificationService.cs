﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Domain.Memberships;
using ChatJS.WebServer;
using ChatJS.WebServer.Hubs;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatJS.WebServer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<ChatHub> _context;
        private readonly IHubConnectionMapper<Guid> _connections;
        private readonly IHubSubscriptionMapper<Guid> _subscriptions;
        private readonly ApplicationDbContext _dbContext;

        public NotificationService(
            IHubContext<ChatHub> context,
            IHubConnectionMapper<Guid> connections,
            IHubSubscriptionMapper<Guid> subscriptions,
            ApplicationDbContext dbContext)
        {
            _context = context;
            _connections = connections;
            _subscriptions = subscriptions;
            _dbContext = dbContext;
        }

        public async Task PublishAsync<T>(string methodName, Guid chatroomId, T content)
        {
            await Task.WhenAll((await _subscriptions.GetSubscribersAsync(chatroomId)).Select(subscriberId =>
            {
                return _context.Clients.Client(subscriberId).SendAsync($"{methodName} | ChatroomId: {chatroomId}", content);
            }));
        }

        public async Task SignInAsync(string subscriberId, Guid userId)
        {
            await _connections.AddAsync(subscriberId, userId);
            await _subscriptions.AddSubscriberAsync(subscriberId);

            var memberships = await _dbContext.Memberships
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == MembershipStatusType.Active)
                .ToListAsync();

            foreach (var membership in memberships)
            {
                await SubscribeAsync(subscriberId, membership.ChatroomId);
            }
        }

        public async Task SignOffAsync(string subscriberId)
        {
            await _connections.RemoveAsync(subscriberId);
            await _subscriptions.RemoveSubscriberAsync(subscriberId);
        }

        public async Task SubscribeAsync(string subscriberId, Guid chatroomId)
        {
            await _subscriptions.SubscribeAsync(subscriberId, chatroomId);
        }

        public async Task UnsubscribeAsync(string subscriberId, Guid chatroomId)
        {
            await _subscriptions.UnsubscribeAsync(subscriberId, chatroomId);
        }
    }
}
