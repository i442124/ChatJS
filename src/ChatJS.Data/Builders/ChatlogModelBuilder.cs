using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Models.Chatlogs;
using ChatJS.Models.Chatrooms;
using ChatJS.Models.Messages;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class ChatlogModelBuilder : IChatlogModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public ChatlogModelBuilder(
            ICacheManager cacheManger,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManger;
        }

        public Task<ChatlogPageModel> BuildChatlogPageModelAsync(Guid userId, Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatlog(userId, chatroomId), async () =>
            {
                var posts = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.CreatedByUser)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .ToListAsync();

                return new ChatlogPageModel
                {
                    Messages = posts.Select(post => _cacheManager.GetOrSet(
                        CacheKeyCollection.ChatlogEntry(userId, chatroomId, post.MessageId), () =>
                        {
                            return new ChatlogPageModel.MessageModel
                            {
                                Id = post.Message.Id,
                                Content = post.Message.Content,
                                TimeStamp = post.Message.CreatedAt,
                                Creator = new ChatlogPageModel.UserModel
                                {
                                    Name = post.Message.CreatedByUser.DisplayName,
                                    NameUid = post.Message.CreatedByUser.DisplayNameUid
                                },
                                Delivery = BuildChatlogDeliveryModel(post.Message),
                                Origin = post.Message.CreatedBy == userId ? MessageOriginType.Send : MessageOriginType.Received
                            };
                        })).ToList()
                };
            });
        }

        public Task<ChatlogPageModel> BuildChatlogPageModelAnonymousAsync(Guid userId, Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatlog(userId, chatroomId), async () =>
            {
                var posts = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .ToListAsync();

                return new ChatlogPageModel
                {
                    Messages = posts.Select(post => _cacheManager.GetOrSet(
                        CacheKeyCollection.ChatlogEntry(userId, chatroomId, post.MessageId), () =>
                        {
                            return new ChatlogPageModel.MessageModel
                            {
                                Id = post.Message.Id,
                                Content = post.Message.Content,
                                TimeStamp = post.Message.CreatedAt,
                                Delivery = BuildChatlogDeliveryModel(post.Message),
                                Origin = post.Message.CreatedBy == userId ? MessageOriginType.Send : MessageOriginType.Received
                            };
                        })).ToList()
                };
            });
        }

        public Task<ChatlogPageModel.MessageModel> BuildMessageModelAsync(Guid userId, Guid chatroomId, Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.ChatlogEntry(userId, chatroomId, messageId), async () =>
            {
                var post = await _dbContext.Posts
                    .Where(x => x.MessageId == messageId)
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.CreatedByUser)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .FirstOrDefaultAsync();

                return new ChatlogPageModel.MessageModel
                {
                    Id = post.Message.Id,
                    Content = post.Message.Content,
                    TimeStamp = post.Message.CreatedAt,
                    Creator = new ChatlogPageModel.UserModel
                    {
                        Name = post.Message.CreatedByUser.DisplayName,
                        NameUid = post.Message.CreatedByUser.DisplayNameUid
                    },
                    Delivery = BuildChatlogDeliveryModel(post.Message),
                    Origin = post.Message.CreatedBy == userId ? MessageOriginType.Send : MessageOriginType.Received
                };
            });
        }

        public Task<ChatlogPageModel.MessageModel> BuildMessageModelAnonymousAsync(Guid userId, Guid chatroomId, Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.ChatlogEntry(userId, chatroomId, messageId), async () =>
            {
                var post = await _dbContext.Posts
                    .Where(x => x.MessageId == messageId)
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .FirstOrDefaultAsync();

                return new ChatlogPageModel.MessageModel
                {
                    Id = post.Message.Id,
                    Content = post.Message.Content,
                    TimeStamp = post.Message.CreatedAt,
                    Delivery = BuildChatlogDeliveryModel(post.Message),
                    Origin = post.Message.CreatedBy == userId ? MessageOriginType.Send : MessageOriginType.Received
                };
            });
        }

        private static ChatlogPageModel.DeliveryModel BuildChatlogDeliveryModel(Message message)
        {
            return new ChatlogPageModel.DeliveryModel
            {
                HasReadByEveryone = message.Deliveries.All(x =>
                    x.Status == DeliveryStatusType.Read),

                HasReceivedByEveryone = message.Deliveries.All(x =>
                    x.Status == DeliveryStatusType.Read ||
                    x.Status == DeliveryStatusType.Received)
            };
        }
    }
}
