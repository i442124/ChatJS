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

namespace ChatJS.Domain.Tests.Messages.Validators
{
    public class CreateMessageValidatorTests : FixtureBase
    {
        [Fact]
        public void Should_HaveValidationError_When_ContentIsEmpty()
        {
            var command = Fixture.Build<CreateMessage>()
                .With(x => x.Content, string.Empty)
                .Create();

            var userRules = new Mock<IUserRules>();
            var messageValidator = new CreateMessageValidator(userRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_ContentTooLong()
        {
            var command = Fixture.Build<CreateMessage>()
                .With(x => x.Content, new string(Enumerable.Repeat('A', 256).ToArray()))
                .Create();

            var userRules = new Mock<IUserRules>();
            var messageValidator = new CreateMessageValidator(userRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_UserNotValid()
        {
            var command = Fixture.Build<CreateMessage>()
                .With(x => x.Content, string.Empty)
                .Create();

            var userRules = new Mock<IUserRules>();

            userRules
                .Setup(x => x.IsValidAsync(command.UserId))
                .ReturnsAsync(false);

            var messageValidator = new CreateMessageValidator(userRules.Object);
            messageValidator.ShouldHaveValidationErrorFor(x => x.Content, command);
        }
    }
}
