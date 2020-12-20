﻿using System.Threading.Tasks;

using ChatJS.Domain.Users.Commands;

namespace ChatJS.Domain.Users
{
    public interface IUserService
    {
        Task CreateAsync(CreateUser command);

        Task DeleteAsync(DeleteUser command);

        Task UpdateAsync(UpdateUser command);
    }
}