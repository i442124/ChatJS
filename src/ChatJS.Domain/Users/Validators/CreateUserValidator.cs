using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using FluentValidation;

namespace ChatJS.Domain.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator(IUserRules rules)
        {
            RuleFor(c => c.IdentityUserId)
                .NotEmpty()
                .WithMessage("User Identity is required.");

            RuleFor(c => c.DisplayName)
               .NotEmpty()
               .WithMessage("DisplayName is required.")
               .Length(min: 1, max: 50)
               .WithMessage("Display name must be at least 1 and at most 50 characters long.");

            RuleFor(c => c.DisplayNameUid)
                .NotEmpty()
                .WithMessage("DisplayNameUid is required.")
                .Length(exactLength: 5)
                .WithMessage("Display name uid must be exactly 5 characters long.");

            RuleFor(c => c)
                .MustAsync((c, cancellation) => rules.IsDisplayNameUniqueAsync(c.DisplayName, c.DisplayNameUid))
                .WithMessage(c => $"User ${c.DisplayName}#{c.DisplayNameUid} already exists.");
        }
    }
}
