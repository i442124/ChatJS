using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using ChatJS.Data.Caching;
using ChatJS.Data.Services;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace ChatJS.Data.Tests.Services
{
    public class MessageServiceTests : FixtureBase
    {
        [Fact]
        public async Task Should_CreateMessage()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var command = Fixture.Create<CreateMessage>();

            var updateValidator = new Mock<IValidator<UpdateMessage>>();
            var createValidator = new Mock<IValidator<CreateMessage>>();
            createValidator
                .Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var cachemanager = new Mock<ICacheManager>();
            var messageService = new MessageService(
                cachemanager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object
            );

            await messageService.CreateAsync(command);
            var messageResult = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(messageResult);
            Assert.Equal(command.Id, messageResult.Id);
            Assert.Equal(command.Content, messageResult.Content);
            Assert.Equal(command.UserId, messageResult.CreatedBy);
            Assert.Equal(MessageStatusType.Published, messageResult.Status);
        }

        [Fact]
        public async Task Should_UpdateMessage()
        {
            var messageId = Guid.NewGuid();
            var message = new Message
            {
                Id = messageId,
                Content = "SomeContent",
            };

            var command = Fixture.Build<UpdateMessage>()
                .With(x => x.Id, messageId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(message);
            await dbContext.SaveChangesAsync();

            var createValidator = new Mock<IValidator<CreateMessage>>();
            var updateValidator = new Mock<IValidator<UpdateMessage>>();
            updateValidator
                .Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var cacheManager = new Mock<ICacheManager>();
            var messageService = new MessageService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await messageService.UpdateAsync(command);
            var messageResult = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(messageResult);
            Assert.Equal(command.Id, messageResult.Id);
            Assert.Equal(command.Content, messageResult.Content);
        }

        [Fact]
        public async Task Should_DeleteMessage()
        {
            var messageId = Guid.NewGuid();
            var message = new Message
            {
                Id = messageId,
                Content = "SomeContent",
            };

            var command = Fixture.Build<DeleteMessage>()
                .With(x => x.Id, messageId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(message);
            await dbContext.SaveChangesAsync();

            var createValidator = new Mock<IValidator<CreateMessage>>();
            var updateValidator = new Mock<IValidator<UpdateMessage>>();
            var cacheManager = new Mock<ICacheManager>();

            var userService = new MessageService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await userService.DeleteAsync(command);
            var messageResult = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(messageResult);
            Assert.Equal(MessageStatusType.Deleted, messageResult.Status);
        }
    }
}
