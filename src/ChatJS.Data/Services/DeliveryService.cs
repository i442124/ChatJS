using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Deliveries.Commands;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public DeliveryService(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task CreateAsync(CreateDelivery command)
        {
            var delivery = new Delivery
            {
                Id = command.Id,
                UserId = command.UserId,
                MessageId = command.MessageId,
                Status = DeliveryStatusType.Pending
            };

            await _dbContext.AddAsync(delivery);
            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Message(delivery.MessageId));
        }

        public async Task<Delivery> GetByIdAsync(GetDeliveryById command)
        {
            var delivery = await _dbContext.Deliveries
                .FirstOrDefaultAsync(x =>
                    x.Id == command.Id);

            if (delivery == null)
            {
                throw new DataException($"Delivery with id {command.Id} not found.");
            }

            return delivery;
        }

        public async Task<Delivery> GetByIdsAsync(GetDeliveryByIds command)
        {
            var delivery = await _dbContext.Deliveries
                .FirstOrDefaultAsync(x =>
                    x.UserId == command.UserId &&
                    x.MessageId == command.MessageId);

            if (delivery == null)
            {
                throw new DataException($"No delivery of message {command.MessageId} to user {command.UserId} found.");
            }

            return delivery;
        }

        public async Task MarkAsReadAsync(MarkDeliveryAsRead command)
        {
            var deliveryById = new GetDeliveryById { Id = command.Id };
            var delivery = await GetByIdAsync(deliveryById);

            if (delivery.Status != DeliveryStatusType.Received)
            {
                throw new DataException($"Delivery {command.Id} is not in a received state.");
            }

            delivery.ReadAt = DateTime.UtcNow;
            delivery.Status = DeliveryStatusType.Read;

            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Message(delivery.MessageId));
        }

        public async Task MarkAsReceivedAsync(MarkDeliveryAsReceived command)
        {
            var deliveryById = new GetDeliveryById { Id = command.Id };
            var delivery = await GetByIdAsync(deliveryById);

            if (delivery.Status != DeliveryStatusType.Pending)
            {
                throw new DataException($"Delivery {command.Id} is not in a pending state.");
            }

            delivery.ReceivedAt = DateTime.UtcNow;
            delivery.Status = DeliveryStatusType.Received;

            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeyCollection.Message(delivery.MessageId));
        }
    }
}
