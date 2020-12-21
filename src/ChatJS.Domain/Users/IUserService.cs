using System;
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

        Task DeleteAsync(DeleteUser command);

        Task<User> GetByIdAsync(GetUserById command);

        Task<User> GetByNameAsync(GetUserByName command);

        Task UpdateAsync(UpdateUser command);
    }
}
