using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Domain.Users;

using FluentValidation;

namespace ChatJS.Domain.Messages.Validators
{
    public class CreateMessageValidator : AbstractValidator<CreateMessage>
    {
        public CreateMessageValidator(IUserRules userRules)
        {
            RuleFor(c => c.Content)
                .NotEmpty()
                .WithMessage("Message content is required.")
                .Length(min: 1, max: 255)
                .WithMessage("Content must be at least 1 and at most 255 characters long.");

            RuleFor(c => c.UserId)
                .MustAsync((id, cancellation) => userRules.IsValidAsync(id))
                .WithMessage(c => $"User with id '{c.UserId}' does not exist.");
        }
    }
}
