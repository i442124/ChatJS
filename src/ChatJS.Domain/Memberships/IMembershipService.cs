using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;

namespace ChatJS.Domain.Memberships
{
    public interface IMembershipService
    {
        Task CreateAsync(CreateMembership command);

        Task SuspendAsync(SuspendMembership command);

        Task ReinstateAsync(ReinstateMembership command);
    }
}
