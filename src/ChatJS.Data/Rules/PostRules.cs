using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class PostRules : IPostRules
    {
        private readonly ApplicationDbContext _dbContext;

        public PostRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            var any = await _dbContext.Posts
                .AnyAsync(post =>
                    post.Id == id &&
                    post.Status == PostStatusType.Published);

            return any;
        }

        public async Task<bool> IsValidAsync(Guid chatroomId, Guid messageId)
        {
            var any = await _dbContext.Posts
                .AnyAsync(post =>
                    post.MessageId == messageId &&
                    post.ChatroomId == chatroomId);

            return any;
        }

        public async Task<bool> IsAuthorizedForPostAsync(Guid userId, Guid postId)
        {
            var any = await _dbContext.Posts
                .AnyAsync(post =>
                    post.Id == postId &&
                    post.Status == PostStatusType.Published &&
                    post.Chatroom.Memberships
                        .Any(membership =>
                            membership.UserId == userId &&
                            membership.Status == MembershipStatusType.Active));

            return any;
        }

        public async Task<bool> IsAuthorizedForChatroomAsync(Guid userId, Guid chatroomId)
        {
            var any = await _dbContext.Memberships
                .AnyAsync(membership =>
                    membership.UserId == userId &&
                    membership.ChatroomId == chatroomId &&
                    membership.Status == MembershipStatusType.Active);

            return any;
        }

        public async Task<bool> IsAuthorizedToPostAsync(Guid userId, Guid messageId)
        {
            var any = await _dbContext.Messages
                .AnyAsync(message =>
                    message.CreatedBy == userId &&
                    message.Status == MessageStatusType.Published);

            return any;
        }
    }
}
