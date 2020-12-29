using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

namespace ChatJS.Domain.Users
{
    public interface IUserService
    {
        Task ConfirmAsync(ConfirmUser command);

        Task CreateAsync(CreateUser command);

        Task UpdateAsync(UpdateUser command);

        Task ReinstateAsync(ReinstateUser command);

        Task SuspendAsync(SuspendUser command);

        Task DeleteAsync(DeleteUser command);
    }
}
