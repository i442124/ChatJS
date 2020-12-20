using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Memberships
{
    public interface IMembershipRules
    {
        Task<bool> IsValidAsync(Guid userId, Guid chatlogId);
    }
}
