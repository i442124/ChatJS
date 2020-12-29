using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Chatrooms
{
    public interface IChatroomRules
    {
        Task<bool> IsValidAsync(Guid id);
    }
}
