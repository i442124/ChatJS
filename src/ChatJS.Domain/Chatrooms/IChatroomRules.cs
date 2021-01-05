using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Chatrooms
{
    public interface IChatroomRules
    {
        Task<bool> IsValidAsync(Guid chatroomId);

        Task<bool> IsAuthorizedAsync(Guid userId, Guid chatroomId);
    }
}
