using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatlogs
{
    public interface IChatlogModelBuilder
    {
        Task<ChatlogPageModel> BuildChatlogPageModelAsync(Guid userId, Guid chatroomId);

        Task<ChatlogPageModel> BuildChatlogPageModelAnonymousAsync(Guid userId, Guid chatroomId);

        Task<ChatlogPageModel.MessageModel> BuildMessageModelAsync(Guid userId, Guid chatroomId, Guid messageId);

        Task<ChatlogPageModel.MessageModel> BuildMessageModelAnonymousAsync(Guid userId, Guid chatroomId, Guid messageId);
    }
}
