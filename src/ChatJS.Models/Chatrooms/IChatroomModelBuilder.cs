using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatrooms
{
    public interface IChatroomModelBuilder
    {
        Task<ChatroomPageModel> BuildChatroomPageModelAsync(Guid chatroomId);

        Task<ChatroomPageModel.ChatroomModel> BuildChatroomModelAsync(Guid chatroomId);
    }
}
