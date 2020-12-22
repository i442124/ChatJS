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

        public async Task CreateAsync(CreateMembership command)
        {
            var result = await _createValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var membership = new Membership
                {
                    UserId = command.UserId,
                    ChatlogId = command.ChatlogId,
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

        public async Task DeleteAsync(DeleteMembership command)
        {
            var membershipById = new GetMembershipById { ChatlogId = command.ChatlogId, UserId = command.UserId };
            var membership = await GetByIdAsync(membershipById);

            membership.Status = MembershipStatusType.Suspended;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Membership> GetByIdAsync(GetMembershipById command)
        {
            var membership = await _dbContext.Memberships
                .FirstOrDefaultAsync(x =>
                    x.UserId == command.UserId &&
                    x.ChatlogId == command.ChatlogId);

            if (membership == null)
            {
                throw new DataException($"Membership was not found.");
            }
            else if (membership.Status == MembershipStatusType.Suspended)
            {
                throw new DataException("$Membership has been suspended.");
            }

            return membership;
        }

        public async Task UpdateAsync(UpdateMembership command)
        {
            var result = await _updateValidator.ValidateAsync(command);
            if (result.IsValid)
            {
                var membershipById = new GetMembershipById { ChatlogId = command.ChatlogId, UserId = command.ChatlogId };
                var membership = await GetByIdAsync(membershipById);

                membership.Status = command.Status;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
