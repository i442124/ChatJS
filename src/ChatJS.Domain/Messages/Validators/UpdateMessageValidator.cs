using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Domain.Users;

using FluentValidation;

namespace ChatJS.Domain.Messages.Validators
{
    public class UpdateMessageValidator : AbstractValidator<UpdateMessage>
    {
        public UpdateMessageValidator(IMessageRules rules)
        {
            RuleFor(c => c.Content)
                .NotEmpty()
                .WithMessage("Message content is required.")
                .Length(min: 1, max: 255)
                .WithMessage("Content must be at least 1 and at most 255 characters long.");

            RuleFor(c => c.Id)
                .MustAsync((id, cancellation) => rules.IsValidAsync(id))
                .WithMessage(c => $"Message with id '{c.Id}' does not exist.");
        }
    }
}
