using System.Threading.Tasks;

using ChatJS.WebServer.Models.Messages.Commands;

namespace ChatJS.WebServer.Models.Messages
{
    public interface IMessageService
    {
        Task DeleteAsync(DeleteMessage message);

        Task<string> CreateAsync(CreateMessage message);

        Task<string> UpdateAsync(UpdateMessage message);
    }
}
