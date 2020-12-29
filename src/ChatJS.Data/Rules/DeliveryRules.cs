using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Deliveries;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class DeliveryRules : IDeliveryRules
    {
        private readonly ApplicationDbContext _dbContext;

        public DeliveryRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            var any = await _dbContext.Deliveries
                .AnyAsync(delivery =>
                    delivery.Id == id);

            return any;
        }

        public async Task<bool> IsValidAsync(Guid userId, Guid messageId)
        {
            var any = await _dbContext.Deliveries
                .AnyAsync(delivery =>
                    delivery.UserId == userId &&
                    delivery.MessageId == messageId);

            return any;
        }
    }
}
