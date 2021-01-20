using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Rules;
using ChatJS.Domain.Users;

using Xunit;

namespace ChatJS.Data.Tests.Rules
{
    public class UserRulesTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_IsValid()
        {
            var dbUserId = Guid.NewGuid();
            var dbUserDisplayName = "Display Name";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Users.Add(new User { Id = dbUserId, DisplayName = dbUserDisplayName });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var userRules = new UserRules(dbContext);
                var userResult = await userRules.IsValidAsync(dbUserId);
                Assert.False(userResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_IsNotValid()
        {
            var dbUserId = Guid.NewGuid();
            var dbUserDisplayName = "Display Name";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Users.Add(new User { Id = dbUserId, DisplayName = dbUserDisplayName });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var userRules = new UserRules(dbContext);
                var userResult = await userRules.IsValidAsync(Guid.NewGuid());
                Assert.False(userResult);
            }
        }

        [Fact]
        public async Task Should_ReturnTrue_When_DisplayNameIsUnique()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var userRules = new UserRules(dbContext);
            var userResult = await userRules.IsDisplayNameUniqueAsync("Display Name");

            Assert.True(userResult);
        }

        [Fact]
        public async Task Should_ReturnTrue_When_DisplayNameIsUnique_WithUid()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var userRules = new UserRules(dbContext);
            var userResult = await userRules.IsDisplayNameUniqueAsync("Display Name", "#0000");

            Assert.True(userResult);
        }

        [Fact]
        public async Task Should_ReturnTrue_When_DisplaynameIsNotUniuqe_WithUid()
        {
            var dbUserDisplayName = "Display Name";
            var dbUserDisplayNameUid = "#0001";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    DisplayName = dbUserDisplayName,
                    DisplayNameUid = dbUserDisplayNameUid,
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var userRules = new UserRules(dbContext);
                var userResult = await userRules.IsDisplayNameUniqueAsync(dbUserDisplayName, "#0002");
                Assert.True(userResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_DisplayNameIsNotUnique()
        {
            var dbUserDisplayName = "Display Name";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Users.Add(new User { Id = Guid.NewGuid(), DisplayName = dbUserDisplayName });
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var userRules = new UserRules(dbContext);
                var userResult = await userRules.IsDisplayNameUniqueAsync(dbUserDisplayName);
                Assert.False(userResult);
            }
        }

        [Fact]
        public async Task Should_ReturnFalse_When_DisplayNameIsNotUnique_WithUid()
        {
            var dbUserDisplayName = "Display Name";
            var dbUserDisplayNameUid = "#0001";
            var dbContextOptions = DbContextOptionsProvider.InMemory;

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                dbContext.Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    DisplayName = dbUserDisplayName,
                    DisplayNameUid = dbUserDisplayNameUid
                });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new ApplicationDbContext(dbContextOptions))
            {
                var userRules = new UserRules(dbContext);
                var userResult = await userRules.IsDisplayNameUniqueAsync(dbUserDisplayName, dbUserDisplayNameUid);
                Assert.False(userResult);
            }
        }
    }
}
