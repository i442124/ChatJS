﻿using ChatJS.Domain.Users.Commands;

using FluentValidation;

namespace ChatJS.Domain.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public UpdateUserValidator(IUserRules rules)
        {
            RuleFor(c => c.DisplayName)
               .NotEmpty()
               .WithMessage("DisplayName is required.")
               .Length(min: 1, max: 50)
               .WithMessage("Display name must be at least 1 and at most 50 characters long.");

            RuleFor(c => c.DisplayNameUid)
                .NotEmpty()
                .WithMessage("DisplayNameUid is required.")
                .Length(exactLength: 4)
                .WithMessage("Display name must be exactly 4 numbers long.")
                .MustAsync((c, d, cancellation) => rules.IsDisplayNameUniqueAsync(c.DisplayName, d))
                .WithMessage(c => $"A user with display name uid {c.DisplayName}#{c.DisplayNameUid} already exists.");
        }
    }
}
