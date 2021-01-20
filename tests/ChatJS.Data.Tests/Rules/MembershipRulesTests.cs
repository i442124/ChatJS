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
    public class MembershipRulesTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_IsValid()
        {
            var dbUserId = Guid.NewGuid();
            var dbChatroomId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
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
                var membershipRules = new MembershipRules(dbContext);
                var membershipResult = await membershipRules.IsValidAsync(dbUserId, dbChatroomId);
                Assert.True(membershipResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_UserId_IsNotValid()
        {
            var dbUserId = Guid.NewGuid();
            var dbChatroomId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
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
                var membershipRules = new MembershipRules(dbContext);
                var membershipResult = await membershipRules.IsValidAsync(Guid.NewGuid(), dbChatroomId);
                Assert.False(membershipResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_ChatroomId_IsNotValid()
        {
            var dbUserId = Guid.NewGuid();
            var dbChatroomId = Guid.NewGuid();
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
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
                var membershipRules = new MembershipRules(dbContext);
                var membershipResult = await membershipRules.IsValidAsync(dbUserId, Guid.NewGuid());
                Assert.False(membershipResult);
            }
        }
    }
}
