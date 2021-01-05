using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Users;

namespace ChatJS.WebServer.Services
{
    public interface INotificationService
    {
        Task PublishAsync<T>(string methodName, Guid chatroomId, T content);

        Task PublishScopedAsync<T>(string methodName, Guid chatroomId, T content);

        Task SignInAsync(string subscriberId, Guid userId);

        Task SignOffAsync(string subscriberId);

        Task SubscribeAsync(Guid userId, Guid chatroomId);

        Task UnsubscribeAsync(Guid userId, Guid chatroomId);
    }
}
