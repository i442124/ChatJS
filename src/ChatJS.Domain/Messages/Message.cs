using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Messages
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }

        public Guid CreatedBy { get; set; }

        public User CreatedByUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public MessageStatusType Status { get; set; }

        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}
