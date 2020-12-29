using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Messages
{
    public interface IMessageRules
    {
        Task<bool> IsValidAsync(Guid id);
    }
}
