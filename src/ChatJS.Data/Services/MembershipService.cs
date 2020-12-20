using System.Data;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateMembership> _createValidator;
        private readonly IValidator<UpdateMembership> _updateValidator;

        public MembershipService(
            ApplicationDbContext dbContext,
            IValidator<CreateMembership> createValidator,
            IValidator<UpdateMembership> updateValidator)
        {
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task CreateMembership(CreateMembership command)
        {
            var result = await _createValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var membership = new Membership
                {
                    ChatlogId = command.ChatlogId,
                    UserId = command.UserId,
                    Status = MembershipStatusType.Active
                };

                await _dbContext.Memberships.AddAsync(membership);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new ValidationException(result.Errors);
            }
        }

        public async Task DeleteMembership(DeleteMembership command)
        {
            var membership = await _dbContext.Memberships
                   .FirstOrDefaultAsync(x =>
                       x.Status != MembershipStatusType.Suspended &&
                       x.ChatlogId == command.ChatlogId &&
                       x.UserId == command.UserId);

            if (membership == null)
            {
                throw new DataException($"Membership with Chatlog '{command.ChatlogId} and User '{command.UserId}' was not found");
            }

            membership.Status = MembershipStatusType.Suspended;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateMembership(UpdateMembership command)
        {
            var result = await _updateValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var membership = await _dbContext.Memberships
                    .FirstOrDefaultAsync(x =>
                        x.Status != MembershipStatusType.Suspended &&
                        x.ChatlogId == command.ChatlogId &&
                        x.UserId == command.UserId);

                if (membership == null)
                {
                    throw new DataException($"Membership with Chatlog '{command.ChatlogId} and User '{command.UserId}' was not found");
                }

                membership.UserId = command.UserId;
                membership.ChatlogId = command.ChatlogId;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
