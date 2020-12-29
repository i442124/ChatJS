using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Posts;
using ChatJS.Domain.Posts.Commands;

namespace ChatJS.Domain.Posts
{
    public interface IPostService
    {
        Task CreateAsync(CreatePost command);

        Task DeleteAsync(DeletePost command);
    }
}
