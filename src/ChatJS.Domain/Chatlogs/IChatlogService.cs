using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Chatlogs.Commands;

namespace ChatJS.Domain.Chatlogs
{
    public interface IChatlogService
    {
        Task CreateAsync(CreateChatlog command);

        Task DeleteAsync(DeleteChatlog command);

        Task<Chatlog> GetByIdAsync(GetChatlogById command);
    }
}
