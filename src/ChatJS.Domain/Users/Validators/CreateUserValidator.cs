using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using FluentValidation;

namespace ChatJS.Domain.Users.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(c => c.IdentityUserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
}
