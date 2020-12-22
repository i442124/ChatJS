using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Models.Messages
{
    public interface IMessageModelBuilder
    {
        Task<MessageAreaModel> BuildAreaAsync(Guid userId, Guid chatlogId);

        Task<MessageEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId, int index);
    }
}
