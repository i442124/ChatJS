using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatrooms
{
    public interface IChatroomModelBuilder
    {
        Task<List<ChatroomReadDto>> BuildAllAsync(Guid userId);

        Task<ChatroomReadDto> BuildAsync(Guid userId, Guid chatroomId);
    }
}
