using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Messages
{
    public class MessagePageModel
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }

        public UserModel Creator { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public List<DeliveryModel> Deliveries { get; set; }

        public class DeliveryModel
        {
            public Guid Id { get; set; }

            public bool HasRead { get; set; }

            public bool HasReceived { get; set; }

            public DateTime ReadAt { get; set; }

            public DateTime ReceivedAt { get; set; }
        }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }

            public string NameCaption { get; set; }
        }
    }
}
