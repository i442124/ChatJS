using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Posts
{
    public interface IPostRules
    {
        Task<bool> IsValidAsync(Guid id);

        Task<bool> IsValidAsync(Guid chatroomId, Guid messageId);
    }
}
