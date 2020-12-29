using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatJS.Domain.Deliveries
{
    public interface IDeliveryRules
    {
        Task<bool> IsValidAsync(Guid id);

        Task<bool> IsValidAsync(Guid userId, Guid messageId);
    }
}
