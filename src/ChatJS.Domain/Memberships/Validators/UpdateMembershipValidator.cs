using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Domain.Users;

using FluentValidation;

namespace ChatJS.Domain.Memberships.Validators
{
    public class UpdateMembershipValidator : AbstractValidator<UpdateMembership>
    {
        public UpdateMembershipValidator(IMembershipRules membershipRules)
        {
            RuleFor(c => c)
                .MustAsync((c, _, cancellation) => membershipRules.IsValidAsync(c.UserId, c.ChatlogId))
                .WithMessage(c => $"Membership with chatlog '{c.ChatlogId}', user '{c.UserId}' is not in a valid state.");
        }
    }
}
