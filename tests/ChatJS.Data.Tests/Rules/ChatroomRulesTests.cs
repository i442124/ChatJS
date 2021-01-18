using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Rules;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Users;

using Xunit;

namespace ChatJS.Data.Tests.Rules
{
    public class ChatroomRulesTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_IsValid()
        {
            var dbChatroomId = Guid.NewGuid();
            var dbChatroomName = "Chatroom Name";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbChatroomId,
                    Name = dbChatroomName,
                    Status = ChatroomStatusType.Active
                });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var chatroomRules = new ChatroomRules(dbContext);
                var chatroomResults = await chatroomRules.IsValidAsync(dbChatroomId);
                Assert.True(chatroomResults);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_IsAuthorized()
        {
            var dbChatroomId = Guid.NewGuid();
            var dbChatroomName = "Chatroom Name";

            var dbUserId = Guid.NewGuid();
            var dbUserDisplayName = "Display Name";

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbChatroomId,
                    Name = dbChatroomName,
                    Status = ChatroomStatusType.Active
                });

                dbContext.Users.Add(new User
                {
                    Id = dbUserId,
                    DisplayName = dbUserDisplayName,
                    Status = UserStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbUserId,
                    ChatroomId = dbChatroomId,
                    Status = MembershipStatusType.Active
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var chatroomRules = new ChatroomRules(dbContext);
                var result = await chatroomRules.IsAuthorizedAsync(dbUserId, dbChatroomId);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotValid()
        {
            var dbChatroomId = Guid.NewGuid();
            var dbChatroomName = "Chatroom Name";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbChatroomId,
                    Name = dbChatroomName,
                    Status = ChatroomStatusType.Active
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var chatroomRules = new ChatroomRules(dbContext);
                var chatroomResult = await chatroomRules.IsValidAsync(Guid.NewGuid());
                Assert.False(chatroomResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotAuthorized()
        {
            var dbChatroomId = Guid.NewGuid();
            var dbChatroomName = "Chatroom Name";

            var dbUserId = Guid.NewGuid();
            var dbUserDisplayName = "Display Name";

            var dbContextOptions = DbContextOptionsProvider.InMemory;
            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Chatrooms.Add(new Chatroom
                {
                    Id = dbChatroomId,
                    Name = dbChatroomName,
                    Status = ChatroomStatusType.Active
                });

                dbContext.Users.Add(new User
                {
                    Id = dbUserId,
                    DisplayName = dbUserDisplayName,
                    Status = UserStatusType.Active
                });

                dbContext.Memberships.Add(new Membership
                {
                    UserId = dbUserId,
                    ChatroomId = dbChatroomId,
                    Status = MembershipStatusType.Active
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var chatroomRules = new ChatroomRules(dbContext);
                var chatroomResult = await chatroomRules.IsAuthorizedAsync(Guid.NewGuid(), dbChatroomId);
                Assert.False(chatroomResult);
            }
        }
    }
}
