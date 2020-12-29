using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
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
    }
}
