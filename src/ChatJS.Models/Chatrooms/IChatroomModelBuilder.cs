using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatrooms
{
    public interface IChatroomModelBuilder
    {
        Task<ChatroomPageModel> BuildChatroomPageModelAsync(Guid userId);

        Task<ChatroomPageModel.ChatroomModel> BuildChatroomModelAsync(Guid userId, Guid chatroomId);
    }
}
