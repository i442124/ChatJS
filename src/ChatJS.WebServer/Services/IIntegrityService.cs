using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace ChatJS.WebServer.Services
{
    public interface IIntegrityService
    {
        Task EnsureUserCreatedAsync(IdentityUser identityUser);

        Task EnsureUserConfirmedAsync(IdentityUser identityUser);
    }
}
