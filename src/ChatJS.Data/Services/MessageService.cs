using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;

namespace ChatJS.Data.Services
{
    public class MessageService : IMessageService
    {
        public Task<string> CreateAsync(CreateMessage message)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(DeleteMessage message)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> UpdateAsync(UpdateMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
