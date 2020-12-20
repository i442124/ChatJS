using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

namespace ChatJS.Domain.Messages
{
    public interface IMessageService
    {
        Task DeleteAsync(DeleteMessage message);

        Task<string> CreateAsync(CreateMessage message);

        Task<string> UpdateAsync(UpdateMessage message);
    }
}
