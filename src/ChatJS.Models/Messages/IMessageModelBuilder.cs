using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Messages
{
    public interface IMessageModelBuilder
    {
        Task<MessagePageModel> BuildMessagePageModelAsync(Guid messageId);
    }
}
