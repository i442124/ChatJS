using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

using FluentValidation;

namespace ChatJS.Domain.Messages.Validators
{
    public class CreateMessageValidator : AbstractValidator<CreateMessage>
    {
        public CreateMessageValidator(IMessageRules messageRules)
        {
        }
    }
}
