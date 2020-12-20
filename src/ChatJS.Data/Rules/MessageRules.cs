using System;
using System.Linq;
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

        public Task<bool> IsValidAsync(Guid messageId, Guid chatId)
        {
            return _dbContext.Messages.AnyAsync(x =>
                x.MessageId == messageId &&
                x.ChatId == chatId
            );
        }
    }
}
