using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Messages;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class MessageRules : IMessageRules
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            var any = await _dbContext.Messages
                .AnyAsync(message =>
                    message.Id == id &&
                    message.Status == MessageStatusType.Published);

            return any;
        }
    }
}
