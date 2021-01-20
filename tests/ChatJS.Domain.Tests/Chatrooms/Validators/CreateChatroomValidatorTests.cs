using System.Linq;

using AutoFixture;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;
using ChatJS.Domain.Chatrooms.Validators;

using FluentValidation;
using FluentValidation.TestHelper;

using Xunit;

namespace ChatJS.Domain.Tests.Chatrooms.Validators
{
    public class CreateChatroomValidatorTests : FixtureBase
    {
        [Fact]
        public void Should_HaveValidationError_When_NameIsEmpty()
        {
            var command = Fixture.Build<CreateChatroom>()
                .With(x => x.Name, string.Empty)
                .Create();

            var chatroomValidator = new CreateChatroomValidator();
            chatroomValidator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_NameTooLong()
        {
            var command = Fixture.Build<CreateChatroom>()
               .With(x => x.Name, new string(Enumerable.Repeat('A', 21).ToArray()))
               .Create();

            var chatroomValidator = new CreateChatroomValidator();
            chatroomValidator.ShouldHaveValidationErrorFor(x => x.Name, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_NameCaptionIsEmpty()
        {
            var command = Fixture.Build<CreateChatroom>()
                .With(x => x.NameCaption, new string(Enumerable.Repeat('A', 51).ToArray()))
                .Create();

            var chatroomValidator = new CreateChatroomValidator();
            chatroomValidator.ShouldHaveValidationErrorFor(x => x.NameCaption, command);
        }

        [Fact]
        public void SHould_HaveValidationError_When_NameCaptionTooLong()
        {
            var command = Fixture.Build<CreateChatroom>()
               .With(x => x.NameCaption, new string(Enumerable.Repeat('A', 51).ToArray()))
               .Create();

            var chatroomValidator = new CreateChatroomValidator();
            chatroomValidator.ShouldHaveValidationErrorFor(x => x.NameCaption, command);
        }
    }
}
