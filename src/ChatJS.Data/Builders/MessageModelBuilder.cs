using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Models.Deliveries;
using ChatJS.Models.Messages;
using ChatJS.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class MessageModelBuilder : IMessageModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        private readonly IUserModelBuilder _userBuilder;
        private readonly IDeliveryModelBuilder _deliveryBuilder;

        public MessageModelBuilder(
            ICacheManager cacheManager,
            IUserModelBuilder userBuilder,
            IDeliveryModelBuilder deliveryBuilder,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;

            _userBuilder = userBuilder;
            _deliveryBuilder = deliveryBuilder;
        }

        public Task<MessageReadDto> BuildAsync(Guid userId, Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Message(userId, messageId), async () =>
            {
                var message = await _dbContext.Messages
                    .Where(x => x.Id == messageId)
                    .Where(x => x.Status == MessageStatusType.Published)
                    .Include(x => x.CreatedByUser)
                    .Include(x => x.Deliveries)
                    .FirstOrDefaultAsync();

                return await CreateAsync(userId, message);
            });
        }

        public Task<List<MessageReadDto>> BuildAllAsync(Guid userId, Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Messages(userId, chatroomId), async () =>
            {
                var messages = await _dbContext.Posts
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == PostStatusType.Published)
                    .Include(x => x.Message).Select(x => x.Message).ToListAsync();

                var messageReadDtos = new List<MessageReadDto>();
                foreach (var message in messages)
                {
                    messageReadDtos.Add(await _cacheManager.GetOrSetAsync(
                        CacheKeyCollection.Message(userId, message.Id), () =>
                        {
                            return CreateAsync(userId, message);
                        }));
                }

                return messageReadDtos;
            });
        }

        private async Task<MessageReadDto> CreateAsync(Guid userId, Message message)
        {
            return new MessageReadDto
            {
                Id = message.Id,
                Content = message.Content,
                TimeStamp = message.CreatedAt,
                Origin = message.CreatedBy ==
                    userId ? MessageOriginType.Send
                           : MessageOriginType.Received,

                Creator = await _userBuilder.BuildAsync(message.CreatedBy),
                Deliveries = await _deliveryBuilder.BuildAllAsync(message.Id)
            };
        }
    }
}
