using System;
using System.Threading.Tasks;

namespace ChatJS.Models.Users
{
    public interface IUserModelBuilder
    {
        Task<UserPageModel> BuildUserPageModelAsync();

        Task<UserPageModel> BuildUserPageModelAsync(Guid userId);

        Task<UserPageModel.UserModel> BuildUserModelAsync(Guid userId);
    }
}
