using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Rules;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;

using Xunit;

namespace ChatJS.Data.Tests.Rules
{
    public class MessageRulesTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_IsValid()
        {
            var dbMessageId = Guid.NewGuid();
            var dbMessageAuthorId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbMessageId,
                    Content = "MyMessageContent",
                    CreatedBy = dbMessageAuthorId,
                    Status = MessageStatusType.Published
                });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var messageRules = new MessageRules(dbContext);
                var result = await messageRules.IsValidAsync(dbMessageId);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_IsAuthorized()
        {
            var dbMessageId = Guid.NewGuid();
            var dbMessageAuthorId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbMessageId,
                    Content = "MyMessageContent",
                    CreatedBy = dbMessageAuthorId,
                    Status = MessageStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var messageRules = new MessageRules(dbContext);
                var result = await messageRules.IsAuthorizedAsync(dbMessageAuthorId, dbMessageId);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotValid()
        {
            var dbMessageId = Guid.NewGuid();
            var dbMessageAuthorId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbMessageId,
                    Content = "MyMessageContent",
                    CreatedBy = dbMessageAuthorId,
                    Status = MessageStatusType.Published
                });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var messageRules = new MessageRules(dbContext);
                var result = await messageRules.IsValidAsync(Guid.NewGuid());
                Assert.False(result);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotAuthorized()
        {
            var dbMessageId = Guid.NewGuid();
            var dbMessageAuthorId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Messages.Add(new Message
                {
                    Id = dbMessageId,
                    Content = "MyMessageContent",
                    CreatedBy = dbMessageAuthorId,
                    Status = MessageStatusType.Published
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var messageRules = new MessageRules(dbContext);
                var result = await messageRules.IsAuthorizedAsync(Guid.NewGuid(), dbMessageId);
                Assert.False(result);
            }
        }
    }
}
