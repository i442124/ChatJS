using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreatePost command)
        {
            var post = new Post
            {
                Id = command.Id,
                MessageId = command.MessageId,
                ChatroomId = command.ChatroomId,
                Status = PostStatusType.Published
            };

            await _dbContext.AddAsync(post);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeletePost command)
        {
            var postById = new GetPostById { Id = command.Id };
            var post = await GetByIdAsync(postById);

            post.Status = PostStatusType.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAsync(GetPostById command)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(post =>
                    post.Id == command.Id &&
                    post.Status == PostStatusType.Published);

            if (post == null)
            {
                throw new DataException($"Post with id {command.Id} not found.");
            }

            return post;
        }

        public async Task<Post> GetByIdsAsync(GetPostByIds command)
        {
            var post = await _dbContext.Posts
                .FirstOrDefaultAsync(post =>
                    post.MessageId == command.MessageId &&
                    post.ChatroomId == command.ChatroomId &&
                    post.Status == PostStatusType.Published);

            if (post == null)
            {
                throw new DataException($"Post message {command.MessageId} not found in chatroom {command.ChatroomId}.");
            }

            return post;
        }
    }
}
