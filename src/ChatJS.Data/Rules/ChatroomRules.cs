using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;

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

        public async Task<bool> IsValidAsync(Guid chatroomId)
        {
            var any = await _dbContext.Chatrooms
                .AnyAsync(chatroom =>
                    chatroom.Id == chatroomId &&
                    chatroom.Status == ChatroomStatusType.Active);

            return any;
        }

        public async Task<bool> IsAuthorizedAsync(Guid userId, Guid chatroomId)
        {
            var any = await _dbContext.Chatrooms
                .AnyAsync(chatroom =>
                    chatroom.Id == chatroomId &&
                    chatroom.Status == ChatroomStatusType.Active &&
                    chatroom.Memberships.Any(user =>
                        user.UserId == userId &&
                        user.Status == MembershipStatusType.Active));

            return any;
        }
    }
}
