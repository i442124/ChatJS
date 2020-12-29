using System;

namespace ChatJS.Domain.Deliveries.Commands
{
    public class GetDeliveryByIds
    {
        public Guid UserId { get; set; }

        public Guid MessageId { get; set; }
    }
}
