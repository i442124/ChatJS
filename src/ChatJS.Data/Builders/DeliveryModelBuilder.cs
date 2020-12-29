using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Deliveries;

using ChatJS.Models.Deliveries;
using ChatJS.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class DeliveryModelBuilder : IDeliveryModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserModelBuilder _userBuilder;
        private readonly ApplicationDbContext _dbContext;

        public DeliveryModelBuilder(
            ICacheManager cacheManager,
            IUserModelBuilder userContext,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userBuilder = userContext;
            _cacheManager = cacheManager;
        }

        public Task<List<DeliveryReadDto>> BuildAllAsync(Guid messageId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Delivery(messageId), async () =>
            {
                var deliveries = await _dbContext.Deliveries
                    .Where(x => x.MessageId == messageId)
                    .Where(x => x.Status != DeliveryStatusType.None)
                    .ToListAsync();

                var deliveryReadDtos = new List<DeliveryReadDto>();
                foreach (var delivery in deliveries)
                {
                    var deliveryReadDto = await CreateAsync(delivery);
                    deliveryReadDtos.Add(deliveryReadDto);
                }

                return deliveryReadDtos;
            });
        }

        private async Task<DeliveryReadDto> CreateAsync(Delivery delivery)
        {
            return new DeliveryReadDto
            {
                Id = delivery.Id,
                ReadAt = delivery.ReadAt,
                ReceivedAt = delivery.ReceivedAt,
                HasRead = delivery.Status == DeliveryStatusType.Read,
                HasReceived = delivery.Status == DeliveryStatusType.Read ||
                              delivery.Status == DeliveryStatusType.Received,

                User = await _userBuilder.BuildAsync(delivery.UserId)
            };
        }
    }
}
