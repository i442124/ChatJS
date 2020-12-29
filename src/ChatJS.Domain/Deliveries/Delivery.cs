using System;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Deliveries
{
    public class Delivery
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }

        public Guid MessageId { get; set; }

        public Message Message { get; set; }

        public DateTime ReadAt { get; set; }

        public DateTime ReceivedAt { get; set; }

        public DeliveryStatusType Status { get; set; }
    }
}
