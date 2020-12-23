using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Users
{
    public interface IUserModelBuilder
    {
        Task<UserAreaModel> BuildAreaAsync(Guid userId);
    }
}
