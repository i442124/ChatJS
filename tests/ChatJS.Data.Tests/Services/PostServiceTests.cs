using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using ChatJS.Data.Caching;
using ChatJS.Data.Services;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace ChatJS.Data.Tests.Services
{
    public class PostServiceTests : FixtureBase
    {
        [Fact]
        public async Task Should_CreatePost()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var command = Fixture.Create<CreatePost>();

            var cacheManager = new Mock<ICacheManager>();
            var postService = new PostService(
                cacheManager.Object,
                dbContext);

            await postService.CreateAsync(command);
            var postResult = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(postResult);
            Assert.Equal(command.Id, postResult.Id);
            Assert.Equal(command.MessageId, postResult.MessageId);
            Assert.Equal(command.ChatroomId, postResult.ChatroomId);
            Assert.Equal(PostStatusType.Published, postResult.Status);
        }

        [Fact]
        public async Task Should_DeletePost()
        {
            var postId = Guid.NewGuid();
            var post = new Post
            {
                Id = postId,
                MessageId = Guid.NewGuid(),
                ChatroomId = Guid.NewGuid(),
                Status = PostStatusType.Published
            };

            var command = Fixture.Build<DeletePost>()
                .With(x => x.Id, postId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(post);
            await dbContext.SaveChangesAsync();

            var cacheManager = new Mock<ICacheManager>();
            var postService = new PostService(
                cacheManager.Object,
                dbContext);

            await postService.DeleteAsync(command);
            var postResult = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(postResult);
            Assert.Equal(PostStatusType.Deleted, postResult.Status);
        }
    }
}
