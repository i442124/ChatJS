using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Rules;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Users;

using Xunit;

namespace ChatJS.Data.Tests.Rules
{
    public class PostRulesTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_IsValid()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Posts.Add(new Post
                {
                    Id = dbPostId,
                    MessageId = dbPostMessageId,
                    ChatroomId = dbPostChatroomId,
                    Status = PostStatusType.Published
                });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);

                var postResults_A = await postRules.IsValidAsync(dbPostId);
                var postResults_B = await postRules.IsValidAsync(dbPostChatroomId, dbPostMessageId);

                Assert.True(postResults_A);
                Assert.True(postResults_B);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_IsAuthorizedForPost()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostUserId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbPostChatroomId,
                    Name = "Chatroom Name",
                    NameCaption = "Chatroom Name Caption",
                    Status = ChatroomStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbPostUserId,
                    ChatroomId = dbPostChatroomId,
                    Status = MembershipStatusType.Active,
                });

                dbContext.Posts.Add(new Post
                {
                    Id = dbPostId,
                    MessageId = dbPostMessageId,
                    ChatroomId = dbPostChatroomId,
                    Status = PostStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedForPostAsync(dbPostUserId, dbPostId);
                Assert.True(postResult);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_IsAuthorizedForChatroom()
        {
            var dbPostUserId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbPostChatroomId,
                    Name = "Chatroom Name",
                    NameCaption = "Chatroom Name Caption",
                    Status = ChatroomStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbPostUserId,
                    ChatroomId = dbPostChatroomId,
                    Status = MembershipStatusType.Active
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedForChatroomAsync(dbPostUserId, dbPostChatroomId);
                Assert.True(postResult);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_IsAuthorizedToPostAsync()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostUserId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbPostMessageId,
                    CreatedBy = dbPostUserId,
                    Content = "My Message Content",
                    Status = MessageStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedToPostAsync(dbPostUserId, dbPostMessageId);
                Assert.True(postResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotValid()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Posts.Add(new Post
                {
                    Id = dbPostId,
                    MessageId = dbPostMessageId,
                    ChatroomId = dbPostChatroomId,
                    Status = PostStatusType.Published
                });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);

                var postResults_A = await postRules.IsValidAsync(Guid.NewGuid());
                var postResults_B = await postRules.IsValidAsync(Guid.NewGuid(), Guid.NewGuid());

                Assert.False(postResults_A);
                Assert.False(postResults_B);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotAuthorizedForPost()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostUserId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbPostChatroomId,
                    Name = "Chatroom Name",
                    NameCaption = "Chatroom Name Caption",
                    Status = ChatroomStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbPostUserId,
                    ChatroomId = dbPostChatroomId,
                    Status = MembershipStatusType.Active,
                });

                dbContext.Posts.Add(new Post
                {
                    Id = dbPostId,
                    MessageId = dbPostMessageId,
                    ChatroomId = dbPostChatroomId,
                    Status = PostStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedForPostAsync(Guid.NewGuid(), dbPostId);
                Assert.False(postResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotAuthorizedForChatroom()
        {
            var dbPostUserId = Guid.NewGuid();
            var dbPostChatroomId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbPostChatroomId,
                    Name = "Chatroom Name",
                    NameCaption = "Chatroom Name Caption",
                    Status = ChatroomStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbPostUserId,
                    ChatroomId = dbPostChatroomId,
                    Status = MembershipStatusType.Active
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedForChatroomAsync(Guid.NewGuid(), dbPostChatroomId);
                Assert.False(postResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotAuthorizedToPostAsync()
        {
            var dbPostId = Guid.NewGuid();
            var dbPostUserId = Guid.NewGuid();
            var dbPostMessageId = Guid.NewGuid();

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbPostMessageId,
                    CreatedBy = dbPostUserId,
                    Content = "My Message Content",
                    Status = MessageStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var postRules = new PostRules(dbContext);
                var postResult = await postRules.IsAuthorizedToPostAsync(Guid.NewGuid(), dbPostMessageId);
                Assert.False(postResult);
            }
        }
    }
}
