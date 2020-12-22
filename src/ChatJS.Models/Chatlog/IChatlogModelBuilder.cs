using System;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatlog
{
    public interface IChatlogModelBuilder
    {
        Task<ChatlogAreaModel> BuildAreaAsync(Guid userId);

        Task<ChatlogEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId);
    }
}
