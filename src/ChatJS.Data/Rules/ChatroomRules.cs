using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatrooms;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class ChatroomRules : IChatroomRules
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatroomRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            var any = await _dbContext.Chatrooms
                .AnyAsync(chatroom =>
                    chatroom.Id == id &&
                    chatroom.Status == ChatroomStatusType.Active);

            return any;
        }
    }
}
