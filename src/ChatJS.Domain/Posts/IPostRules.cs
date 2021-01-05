using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Posts
{
    public interface IPostRules
    {
        Task<bool> IsAuthorizedForPostAsync(Guid userId, Guid postId);

        Task<bool> IsAuthorizedForChatroomAsync(Guid userId, Guid chatroomId);

        Task<bool> IsAuthorizedToPostAsync(Guid userId, Guid messageId);

        Task<bool> IsValidAsync(Guid id);

        Task<bool> IsValidAsync(Guid chatroomId, Guid messageId);
    }
}
