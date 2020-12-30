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

        public Task<ChatlogPageModel> BuildChatlogPageModelAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatlog(chatroomId), async () =>
            {
                var posts = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.CreatedByUser)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .OrderBy(x => x.Message.CreatedAt)
                        .ToListAsync();

                return new ChatlogPageModel
                {
                    Messages = posts.Select(post => _cacheManager.GetOrSet(
                        CacheKeyCollection.ChatlogEntry(chatroomId, post.MessageId), () =>
                        {
                            return new ChatlogPageModel.MessageModel
                            {
                                Id = post.Message.Id,
                                Content = post.Message.Content,
                                TimeStamp = post.Message.CreatedAt,
                                Creator = new ChatlogPageModel.UserModel
                                {
                                    Id = post.Message.CreatedByUser.Id,
                                    Name = post.Message.CreatedByUser.DisplayName,
                                    NameUid = post.Message.CreatedByUser.DisplayNameUid
                                },
                                Delivery = BuildChatlogDeliveryModel(post.Message)
                            };
                        })).ToList()
                };
            });
        }

        public Task<ChatlogPageModel> BuildChatlogPageModelAnonymousAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatlog(chatroomId), async () =>
            {
                var posts = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                        .Include(x => x.Message)
                        .ThenInclude(x => x.Deliveries)
                        .OrderBy(x => x.Message.CreatedAt)
                        .ToListAsync();

                return new ChatlogPageModel
                {
                    Messages = posts.Select(post => _cacheManager.GetOrSet(
                        CacheKeyCollection.ChatlogEntry(chatroomId, post.MessageId), () =>
                        {
                            return new ChatlogPageModel.MessageModel
                            {
                                Id = post.Message.Id,
                                Content = post.Message.Content,
                                TimeStamp = post.Message.CreatedAt,
                                Delivery = BuildChatlogDeliveryModel(post.Message)
                            };
                        })).ToList()
                };
            });
        }

        public Task<ChatlogPageModel.MessageModel> BuildMessageModelAsync(Guid chatroomId, Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.ChatlogEntry(chatroomId, messageId), async () =>
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
                        Id = post.Message.CreatedByUser.Id,
                        Name = post.Message.CreatedByUser.DisplayName,
                        NameUid = post.Message.CreatedByUser.DisplayNameUid
                    },
                    Delivery = BuildChatlogDeliveryModel(post.Message),
                };
            });
        }

        public Task<ChatlogPageModel.MessageModel> BuildMessageModelAnonymousAsync(Guid chatroomId, Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.ChatlogEntry(chatroomId, messageId), async () =>
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
                };
            });
        }

        private static ChatlogPageModel.DeliveryModel BuildChatlogDeliveryModel(Message message)
        {
            return new ChatlogPageModel.DeliveryModel
            {
                WasReadByEveryone = message.Deliveries.All(x =>
                    x.Status == DeliveryStatusType.Read),

                WasReceivedByEveryone = message.Deliveries.All(x =>
                    x.Status == DeliveryStatusType.Read ||
                    x.Status == DeliveryStatusType.Received)
            };
        }
    }
}
