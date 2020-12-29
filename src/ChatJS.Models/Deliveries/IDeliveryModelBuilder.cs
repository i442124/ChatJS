using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatJS.Models.Deliveries
{
    public interface IDeliveryModelBuilder
    {
        Task<List<DeliveryReadDto>> BuildAllAsync(Guid messageId);
    }
}
