using System.Linq;

using AutoFixture;

using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;
using ChatJS.Domain.Users.Validators;

using FluentValidation;
using FluentValidation.TestHelper;

using Moq;

using Xunit;

namespace ChatJS.Domain.Tests.Users.Validators
{
    public class CreateUserValidatorTests : FixtureBase
    {
        [Fact]
        public void Should_HaveValidationError_When_UserIdIsEmpty()
        {
            var command = Fixture.Build<CreateUser>()
                .With(x => x.IdentityUserId, string.Empty)
                .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.IdentityUserId, command);
        }

        [Fact]
        public void Should_haveValidationError_When_DisplayNameIsNull()
        {
            var command = Fixture.Build<CreateUser>()
               .With(x => x.DisplayName, (string)null)
               .With(x => x.DisplayNameUid, "#0000")
               .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayName, command);
        }

        [Fact]
        public void Should_HaveValidationError_When_DisplayNameIsEmpty()
        {
            var command = Fixture.Build<CreateUser>()
                .With(x => x.DisplayName, string.Empty)
                .With(x => x.DisplayNameUid, "#0000")
                .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayName, command);
        }

        [Fact]
        public void ShouldHaveValidationError_When_DisplayNameNotUnique()
        {
            var command = Fixture.Build<CreateUser>()
             .With(x => x.DisplayName, "DisplayName")
             .With(x => x.DisplayNameUid, "#0000")
             .Create();

            var userRules = new Mock<IUserRules>();
            userRules.Setup(x => x.IsDisplayNameUniqueAsync(command.DisplayName, command.DisplayNameUid)).ReturnsAsync(false);

            var userValidator = new CreateUserValidator(userRules.Object);
            userValidator.ShouldHaveValidationErrorFor(x => x, value: command);
        }

        [Fact]
        public void Should_HaveValidationError_When_DisplayNameIsTooLong()
        {
            var command = Fixture.Build<CreateUser>()
              .With(x => x.DisplayName, new string(Enumerable.Repeat('A', 51).ToArray()))
              .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayName, command);
        }

        [Fact]
        public void ShouldHaveValidationError_When_DisplayNameUidIsNull()
        {
            var command = Fixture.Build<CreateUser>()
               .With(x => x.DisplayName, "Display Name")
               .With(x => x.DisplayNameUid, (string)null)
               .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayNameUid, command);
        }

        [Fact]
        public void ShouldHaveValidationError_When_DisplayNameUidIsEmpty()
        {
            var command = Fixture.Build<CreateUser>()
               .With(x => x.DisplayName, "Display Name")
               .With(x => x.DisplayNameUid, string.Empty)
               .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayNameUid, command);
        }

        [Fact]
        public void ShouldHaveValidationError_When_DisplayNameUidInvalidFormat()
        {
            var command = Fixture.Build<CreateUser>()
               .With(x => x.DisplayName, "#ABCDE")
               .Create();

            var userRules = new Mock<IUserRules>();
            var userValidator = new CreateUserValidator(userRules.Object);

            userValidator.ShouldHaveValidationErrorFor(x => x.DisplayNameUid, command);
        }
    }
}
