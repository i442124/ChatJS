using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatlogs
{
    public interface IChatlogModelBuilder
    {
        Task<ChatlogPageModel> BuildChatlogPageModelAsync(Guid chatroomId);

        Task<ChatlogPageModel.MessageModel> BuildMessageModelAsync(Guid chatroomId, Guid messageId);
    }
}
