using System;
using System.Threading.Tasks;

namespace ChatJS.Models.Users
{
    public interface IUserModelBuilder
    {
        Task<UserReadDto> BuildAsync(Guid userId);
    }
}
