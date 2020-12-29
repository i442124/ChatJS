using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Deliveries.Commands;

namespace ChatJS.Domain.Deliveries
{
    public interface IDeliveryService
    {
        Task CreateAsync(CreateDelivery command);

        Task MarkAsReadAsync(MarkDeliveryAsRead command);

        Task MarkAsReceivedAsync(MarkDeliveryAsReceived command);
    }
}
