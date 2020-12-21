using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class ChatlogRules : IChatlogRules
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatlogRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid chatlogId)
        {
            var any = await _dbContext.Chatlogs.AnyAsync(
                chatlog => chatlog.Id == chatlogId &&
                           chatlog.Status != ChatlogStatusType.Deleted);

            return any;
        }
    }
}
