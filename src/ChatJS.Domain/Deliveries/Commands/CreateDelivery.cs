using System;

namespace ChatJS.Domain.Deliveries.Commands
{
    public class CreateDelivery
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public Guid MessageId { get; set; }
    }
}
