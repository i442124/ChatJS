using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Models;
using ChatJS.Models.Deliveries;
using ChatJS.Models.Users;

namespace ChatJS.Models.Messages
{
    public class MessageReadDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public UserReadDto Creator { get; set; }

        public MessageOriginType Origin { get; set; }

        public List<DeliveryReadDto> Deliveries { get; set; }
        = new List<DeliveryReadDto>();
    }
}
