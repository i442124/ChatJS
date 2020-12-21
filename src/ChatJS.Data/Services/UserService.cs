using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChatJS.Data.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateUser> _createValidator;
        private readonly IValidator<UpdateUser> _updateValidator;

        public UserService(
            ApplicationDbContext dbContext,
            IValidator<CreateUser> createValidator,
            IValidator<UpdateUser> updateValidator)
        {
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task ConfirmAsync(ConfirmUser command)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(x =>
                    x.Id == command.Id &&
                    x.Status == UserStatusType.Peinding);

            if (user == null)
            {
                throw new DataException($"User with Id '{command.Id}' not found.");
            }

            user.Status = UserStatusType.Active;
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(CreateUser command)
        {
            var result = await _createValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var user = new User
                {
                    DisplayName = command.DisplayName,
                    DisplayNameUid = command.DisplayNameUid,
                    IdentityUserId = command.IdentityUserId,
                    Id = command.Id,
                };

                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task DeleteAsync(DeleteUser command)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Status != UserStatusType.Deleted && x.Id == command.Id);
            if (user == null)
            {
                throw new DataException($"User with Id '{command.Id}' was not found.");
            }

            user.Status = UserStatusType.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateUser command)
        {
            var result = await _updateValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Status != UserStatusType.Deleted && x.Id == command.Id);
                if (user == null)
                {
                    throw new DataException($"User with Id '{command.Id}' was not found.");
                }

                user.DisplayName = command.DisplayName;
                user.DisplayNameUid = command.DisplayNameUid;
                await _dbContext.SaveChangesAsync();
            }
        }

        private async Task<string> GenerateDisplayName()
        {
            var random = new Random();
            var displayNameFound = true;
            var displayName = string.Empty;

            while (displayNameFound)
            {
                displayName = $"<{random.Next()}>";
                displayNameFound = await _dbContext.Users
                    .AnyAsync(x => x.DisplayName == displayName);
            }

            return displayName;
        }

        private async Task<string> GenerateDisplayNameUid(string displayName)
        {
            var random = new Random();
            var displayNameUidFound = true;
            var displayNameUid = string.Empty;

            while (displayNameUidFound)
            {
                displayNameUid = $"{random.Next(9999):D4}";
                displayNameUidFound = await _dbContext.Users
                    .AnyAsync(x => x.DisplayName == displayName &&
                                   x.DisplayNameUid == displayNameUid);
            }

            return displayNameUid;
        }
    }
}
