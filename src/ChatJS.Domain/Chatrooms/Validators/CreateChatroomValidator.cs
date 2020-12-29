using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;

using FluentValidation;

namespace ChatJS.Domain.Chatrooms.Validators
{
    public class CreateChatroomValidator : AbstractValidator<CreateChatroom>
    {
        public CreateChatroomValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().When(c => c.Name != null)
                .WithMessage($"Chatroom name cannot be an empty string.")
                .Length(min: 1, max: 20).When(c => c.Name != null)
                .WithMessage($"Chatroom name must be at least 1 and at most 20 characters long.");

            RuleFor(c => c.NameCaption)
                .NotEmpty().When(c => c.NameCaption != null)
                .WithMessage($"Chatroom name cannot be an empty string.")
                .Length(min: 1, max: 50).When(c => c.NameCaption != null)
                .WithMessage($"Chatroom name must be at least 1 and at most 50 characters long.");
        }
    }
}
