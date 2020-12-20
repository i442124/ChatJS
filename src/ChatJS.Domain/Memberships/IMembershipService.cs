using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

namespace ChatJS.Domain.Memberships
{
    public interface IMembershipService
    {
        Task CreateMembership(CreateMembership command);

        Task DeleteMembership(DeleteMembership command);

        Task UpdateMembership(UpdateMembership command);
    }
}
