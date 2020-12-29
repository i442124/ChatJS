using System;

using ChatJS.Models;
using ChatJS.Models.Users;

namespace ChatJS.Models.Deliveries
{
    public class DeliveryReadDto
    {
        public Guid Id { get; set; }

        public bool HasRead { get; set; }

        public bool HasReceived { get; set; }

        public DateTime ReadAt { get; set; }

        public DateTime ReceivedAt { get; set; }

        public UserReadDto User { get; set; }
    }
}
