using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Chatlogs.Commands;

namespace ChatJS.Domain.Chatlogs
{
    public interface IChatlogService
    {
        Task<Chatlog> GetByIdAsync(GetChatlogById command);
    }
}
