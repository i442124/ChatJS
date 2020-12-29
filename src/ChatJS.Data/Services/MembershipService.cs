using System.Data;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _dbContext;

        public MembershipService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateAsync(CreateMembership command)
        {
            var membership = new Membership
            {
                UserId = command.UserId,
                ChatroomId = command.ChatroomId,
                Status = MembershipStatusType.Active
            };

            await _dbContext.AddAsync(membership);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Membership> GetByIdAsync(GetMembershipById command)
        {
            var membership = await _dbContext.Memberships
                .FirstOrDefaultAsync(membership =>
                    membership.UserId == command.UserId &&
                    membership.ChatroomId == command.ChatroomId &&
                    membership.Status == MembershipStatusType.Active);

            if (membership == null)
            {
                throw new DataException($"Membership with user id {command.UserId} and chatlog id {command.ChatroomId} not found.");
            }

            return membership;
        }

        public async Task ReinstateAsync(ReinstateMembership command)
        {
            var membershipById = new GetMembershipById { UserId = command.UserId, ChatroomId = command.ChatroomId };
            var membership = await GetByIdAsync(membershipById);

            membership.Status = MembershipStatusType.Active;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SuspendAsync(SuspendMembership command)
        {
            var membershipById = new GetMembershipById { UserId = command.UserId, ChatroomId = command.ChatroomId };
            var membership = await GetByIdAsync(membershipById);

            membership.Status = MembershipStatusType.Suspended;
            await _dbContext.SaveChangesAsync();
        }
    }
}
