using System;
using System.Threading.Tasks;

namespace ChatJS.WebServer.Models.Messages
{
    public interface IMessageRules
    {
        Task<bool> IsValidAsync(Guid chatId);
    }
}
