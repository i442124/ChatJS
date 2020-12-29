using System.Threading.Tasks;

using ChatJS.Domain.Messages.Commands;

namespace ChatJS.Domain.Messages
{
    public interface IMessageService
    {
        Task CreateAsync(CreateMessage command);

        Task UpdateAsync(UpdateMessage command);

        Task DeleteAsync(DeleteMessage command);
    }
}
