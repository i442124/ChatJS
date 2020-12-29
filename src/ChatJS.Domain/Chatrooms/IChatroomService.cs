using System.Threading.Tasks;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;

namespace ChatJS.Domain.Chatrooms
{
    public interface IChatroomService
    {
        Task CreateAsync(CreateChatroom command);

        Task UpdateAsync(UpdateChatroom command);

        Task DeleteAsync(DeleteChatroom command);
    }
}
