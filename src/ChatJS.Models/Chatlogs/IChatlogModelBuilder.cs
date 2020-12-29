using System;
using System.Threading.Tasks;

namespace ChatJS.Models.Chatlogs
{
    public interface IChatlogModelBuilder
    {
        Task<ChatlogReadDto> BuildAsync(Guid userId, Guid chatroomId);
    }
}
