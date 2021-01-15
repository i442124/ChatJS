using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using ChatJS.Data.Caching;
using ChatJS.Data.Services;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace ChatJS.Data.Tests.Services
{
    public class UserServiceTests : FixtureBase
    {
        [Fact]
        public async Task Should_CreateUser()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var command = Fixture.Create<CreateUser>();

            var updateValidator = new Mock<IValidator<UpdateUser>>();
            var createValidator = new Mock<IValidator<CreateUser>>();
            createValidator
                .Setup(x => x.ValidateAsync(command, new CancellationToken()))
                .ReturnsAsync(new ValidationResult());

            var cacheManager = new Mock<ICacheManager>();
            var userService = new UserService
                (cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await userService.CreateAsync(command);
            var userResult = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(userResult);
            Assert.Equal(command.Id, userResult.Id);
            Assert.Equal(command.DisplayName, userResult.DisplayName);
            Assert.Equal(command.DisplayNameUid, userResult.DisplayNameUid);
            Assert.Equal(UserStatusType.Pending, userResult.Status);
        }

        [Fact]
        public async Task Should_ConfirmUser()
        {
            var userId = Guid.NewGuid();
            var user = new User {
                Id = userId,
                DisplayName = "Name",
                DisplayNameUid = "#0001",
                Status = UserStatusType.Pending
            };

            var command = Fixture.Build<ConfirmUser>()
                .With(x => x.Id, userId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var updateValidator = new Mock<IValidator<UpdateUser>>();
            var createValidator = new Mock<IValidator<CreateUser>>();
            var cacheManager = new Mock<ICacheManager>();

            var userService = new UserService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await userService.ConfirmAsync(command);
            var userResult = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(userResult);
            Assert.Equal(UserStatusType.Active, userResult.Status);
        }

        [Fact]
        public async Task Should_UpdateUser()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                DisplayName = "Name",
                DisplayNameUid = "#0001",
                Status = UserStatusType.Active
            };

            var command = Fixture.Build<UpdateUser>()
                .With(x => x.Id, userId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var createValidator = new Mock<IValidator<CreateUser>>();
            var updateValidator = new Mock<IValidator<UpdateUser>>();
            updateValidator
                .Setup(x => x.ValidateAsync(command, new CancellationToken()))
                .ReturnsAsync(new ValidationResult());

            var cacheManager = new Mock<ICacheManager>();

            var userService = new UserService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await userService.UpdateAsync(command);
            var userResult = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(userResult);
            Assert.Equal(command.DisplayName, userResult.DisplayName);
            Assert.Equal(command.DisplayNameUid, userResult.DisplayNameUid);
        }

        [Fact]
        public async Task Should_DeleteUser()
        {
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                DisplayName = "Name",
                DisplayNameUid = "#0001",
                Status = UserStatusType.Active
            };

            var command = Fixture.Build<DeleteUser>()
                .With(x => x.Id, userId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var createValidator = new Mock<IValidator<CreateUser>>();
            var updateValidator = new Mock<IValidator<UpdateUser>>();
            var cacheManager = new Mock<ICacheManager>();

            var userService = new UserService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await userService.DeleteAsync(command);
            var userResult = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(userResult);
            Assert.Equal(UserStatusType.Deleted, userResult.Status);
        }
    }
}
