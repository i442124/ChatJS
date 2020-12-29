using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Messages
{
    public interface IMessageModelBuilder
    {
        Task<MessageReadDto> BuildAsync(Guid userId, Guid messageId);

        Task<List<MessageReadDto>> BuildAllAsync(Guid userId, Guid chatroomId);
    }
}
