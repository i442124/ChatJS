using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using ChatJS.Data.Caching;
using ChatJS.Data.Services;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace ChatJS.Data.Tests.Services
{
    public class MembershipServiceTests : FixtureBase
    {
        [Fact]
        public async Task Should_CreateMembership()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var command = Fixture.Create<CreateMembership>();

            var cacheManager = new Mock<ICacheManager>();
            var membershipService = new MembershipService(
                cacheManager.Object,
                dbContext);

            await membershipService.CreateAsync(command);
            var membershipResult = await dbContext.Memberships
                .FirstOrDefaultAsync(membership =>
                    membership.UserId == command.UserId &&
                    membership.ChatroomId == command.ChatroomId);

            Assert.NotNull(membershipResult);
            Assert.Equal(command.UserId, membershipResult.UserId);
            Assert.Equal(command.ChatroomId, membershipResult.ChatroomId);
            Assert.Equal(MembershipStatusType.Active, membershipResult.Status);
        }

        [Fact]
        public async Task Should_SuspendMembership()
        {
            var membershipUserId = Guid.NewGuid();
            var membershipChatroomId = Guid.NewGuid();
            var membership = new Membership
            {
                UserId = membershipUserId,
                ChatroomId = membershipChatroomId,
                Status = MembershipStatusType.Active
            };

            var command = Fixture.Build<SuspendMembership>()
                .With(x => x.ChatroomId, membershipChatroomId)
                .With(x => x.UserId, membershipUserId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(membership);
            await dbContext.SaveChangesAsync();

            var cacheManager = new Mock<ICacheManager>();
            var membershipService = new MembershipService(
                cacheManager.Object,
                dbContext);

            await membershipService.SuspendAsync(command);
            var membershipResult = await dbContext.Memberships
                .FirstOrDefaultAsync(membership =>
                    membership.UserId == command.UserId &&
                    membership.ChatroomId == command.ChatroomId);

            Assert.NotNull(membershipResult);
            Assert.Equal(MembershipStatusType.Suspended, membershipResult.Status);
        }

        [Fact]
        public async Task Should_ReinstateMembership()
        {
            var membershipUserId = Guid.NewGuid();
            var membershipChatroomId = Guid.NewGuid();
            var membership = new Membership
            {
                UserId = membershipUserId,
                ChatroomId = membershipChatroomId,
                Status = MembershipStatusType.Suspended
            };

            var command = Fixture.Build<ReinstateMembership>()
                .With(x => x.ChatroomId, membershipChatroomId)
                .With(x => x.UserId, membershipUserId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(membership);
            await dbContext.SaveChangesAsync();

            var cacheManager = new Mock<ICacheManager>();
            var membershipService = new MembershipService(
                cacheManager.Object,
                dbContext);

            await membershipService.ReinstateAsync(command);
            var membershipResult = await dbContext.Memberships
                .FirstOrDefaultAsync(membership =>
                    membership.UserId == command.UserId &&
                    membership.ChatroomId == command.ChatroomId);

            Assert.NotNull(membershipResult);
            Assert.Equal(MembershipStatusType.Active, membershipResult.Status);
        }
    }
}
