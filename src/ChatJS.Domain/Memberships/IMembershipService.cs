using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

namespace ChatJS.Domain.Memberships
{
    public interface IMembershipService
    {
        Task CreateAsync(CreateMembership command);

        Task DeleteAsync(DeleteMembership command);

        Task<Membership> GetByIdAsync(GetMembershipById command);

        Task UpdateAsync(UpdateMembership command);
    }
}
