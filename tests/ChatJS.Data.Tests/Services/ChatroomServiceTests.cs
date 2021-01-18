using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture;

using ChatJS.Data.Caching;
using ChatJS.Data.Services;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.EntityFrameworkCore;

using Moq;

using Xunit;

namespace ChatJS.Data.Tests.Services
{
    public class ChatroomServiceTests : FixtureBase
    {
        [Fact]
        public async Task Should_CreateChatroom()
        {
            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            var command = Fixture.Create<CreateChatroom>();

            var cacheManager = new Mock<ICacheManager>();
            var updateValidator = new Mock<IValidator<UpdateChatroom>>();
            var createValidator = new Mock<IValidator<CreateChatroom>>();

            createValidator
                .Setup(x => x.ValidateAsync(command, CancellationToken.None))
                .ReturnsAsync(new ValidationResult());

            var chatroomService = new ChatroomService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await chatroomService.CreateAsync(command);
            var chatroomResult = await dbContext.Chatrooms.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(chatroomResult);
            Assert.Equal(command.Id, chatroomResult.Id);
            Assert.Equal(command.Name, chatroomResult.Name);
            Assert.Equal(command.NameCaption, chatroomResult.NameCaption);
            Assert.Equal(ChatroomStatusType.Active, chatroomResult.Status);
        }

        [Fact]
        public async Task Should_UpdateChatroom()
        {
            var chatroomId = Guid.NewGuid();
            var chatroom = new Chatroom
            {
                Id = chatroomId,
                Name = "Chatroom Name",
                NameCaption = "Chatroom Name Caption"
            };

            var command = Fixture.Build<UpdateChatroom>()
                .With(x => x.Id, chatroomId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(chatroom);
            await dbContext.SaveChangesAsync();

            var cacheManager = new Mock<ICacheManager>();
            var createValidator = new Mock<IValidator<CreateChatroom>>();
            var updateValidator = new Mock<IValidator<UpdateChatroom>>();

            var chatroomService = new ChatroomService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await chatroomService.UpdateAsync(command);
            var chatroomResult = await dbContext.Chatrooms.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(chatroomResult);
            Assert.Equal(command.Id, chatroomResult.Id);
            Assert.Equal(command.Name, chatroomResult.Name);
            Assert.Equal(command.NameCaption, chatroomResult.NameCaption);
        }

        [Fact]
        public async Task Should_DeleteChatroom()
        {
            var chatroomId = Guid.NewGuid();
            var chatroom = new Chatroom
            {
                Id = chatroomId,
                Name = "Chatroom Name",
                NameCaption = "Chatroom Name Caption"
            };

            var command = Fixture.Build<DeleteChatroom>()
                .With(x => x.Id, chatroomId)
                .Create();

            using var dbContext = new ApplicationDbContext(DbContextOptionsProvider.InMemory);
            await dbContext.AddAsync(chatroom);
            await dbContext.SaveChangesAsync();

            var cacheManager = new Mock<ICacheManager>();
            var createValidator = new Mock<IValidator<CreateChatroom>>();
            var updateValidator = new Mock<IValidator<UpdateChatroom>>();

            var chatroomService = new ChatroomService(
                cacheManager.Object,
                dbContext,
                createValidator.Object,
                updateValidator.Object);

            await chatroomService.DeleteAsync(command);
            var chatroomResult = await dbContext.Chatrooms.FirstOrDefaultAsync(x => x.Id == command.Id);

            Assert.NotNull(chatroomResult);
            Assert.Equal(ChatroomStatusType.Deleted, chatroomResult.Status);
        }
    }
}
