using System.Linq;

using AutoFixture;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Domain.Messages.Validators;
using ChatJS.Domain.Users;

using FluentValidation;
using FluentValidation.TestHelper;

using Moq;

using Xunit;

namespace ChatJS.Domain.Tests.Messages
{
    public class UpdateMessageValidatorTests : FixtureBase
    {
        [Fact]
        public void Should_HaveValidationError_When_ContentIsEmpty()
        {
            var command = Fixture.Build<UpdateMessage>()
                .With(x => x.Content, string.Empty)
                .Create();

            var messageRules = new Mock<IMessageRules>();
            var messageValidator = new UpdateMessageValidator(messageRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_ContentTooLong()
        {
            var command = Fixture.Build<UpdateMessage>()
                .With(x => x.Content, new string(Enumerable.Repeat('A', 256).ToArray()))
                .Create();

            var messageRules = new Mock<IMessageRules>();
            var messageValidator = new UpdateMessageValidator(messageRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_UserNotValid()
        {
            var command = Fixture.Build<UpdateMessage>()
                .With(x => x.Content, string.Empty)
                .Create();

            var messageRules = new Mock<IMessageRules>();

            messageRules
                .Setup(x => x.IsValidAsync(command.Id))
                .ReturnsAsync(false);

            var messageValidator = new UpdateMessageValidator(messageRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }
    }
}
