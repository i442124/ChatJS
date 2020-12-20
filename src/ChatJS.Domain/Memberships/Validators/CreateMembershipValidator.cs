using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Domain.Users;

using FluentValidation;

namespace ChatJS.Domain.Memberships.Validators
{
    public class CreateMembershipValidator : AbstractValidator<CreateMembership>
    {
        public CreateMembershipValidator(
            IUserRules userRules, IChatlogRules chatlogRules)
        {
            RuleFor(c => c.UserId)
                .MustAsync((c, id, cancellation) => userRules.IsValidAsync(id))
                .WithMessage(c => $"User {c.UserId} is not in a valid state.");

            RuleFor(c => c.ChatlogId)
                .MustAsync((c, id, cancellation) => chatlogRules.IsValidAsync(id))
                .WithMessage(c => $"Chatlog {c.ChatlogId} is not in a valid state.");
        }
    }
}
