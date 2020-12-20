using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Users
{
    public interface IUserRules
    {
        Task<bool> IsDisplayNameUniqueAsync(string displayName);

        Task<bool> IsDisplayNameUniqueAsync(string displayName, string displayNameUid);
    }
}
