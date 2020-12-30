using System;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Messages;
using ChatJS.Models.Messages;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class MessageModelBuilder : IMessageModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public MessageModelBuilder(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public Task<MessagePageModel> BuildMessagePageModelAsync(Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Message(messageId), async () =>
            {
                var message = await _dbContext.Messages
                    .Where(x => x.Id == messageId)
                    .Where(x => x.Status != MessageStatusType.Deleted)
                    .Include(x => x.Deliveries)
                    .FirstOrDefaultAsync();

                return new MessagePageModel
                {
                    Id = message.Id,
                    Content = message.Content,
                    Attachment = message.Attachment,
                    Creator = new MessagePageModel.UserModel
                    {
                        Id = message.CreatedBy,
                        Name = message.CreatedByUser.DisplayName,
                        NameUid = message.CreatedByUser.DisplayNameUid,
                        NameCaption = message.CreatedByUser.DisplayNameUid
                    },
                    CreatedAt = message.CreatedAt,
                    ModifiedAt = message.ModifiedAt,
                    Deliveries = message.Deliveries.Select(x =>
                    {
                        return new MessagePageModel.DeliveryModel
                        {
                            Id = x.Id,
                            ReadAt = x.ReadAt,
                            ReceivedAt = x.ReceivedAt,
                            HasRead = x.Status == DeliveryStatusType.Read,
                            HasReceived = x.Status == DeliveryStatusType.Read ||
                                          x.Status == DeliveryStatusType.Received
                        };
                    }).ToList()
                };
            });
        }
    }
}
