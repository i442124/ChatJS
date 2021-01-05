using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Posts
{
    public class PostPageModel
    {
        public List<MessageModel> Messages { get; set; }

        public class ChatroomModel
        {
            public Guid Id { get; set; }
        }

        public class DeliveryModel
        {
            public bool IsReadByEveryone { get; set; }

            public bool IsReceivedByEveryone { get; set; }
        }

        public class MessageModel
        {
            public Guid Id { get; set; }

            public string Content { get; set; }

            public byte[] Attachment { get; set; }

            public UserModel Creator { get; set; }

            public DateTime TimeStamp { get; set; }

            public DeliveryModel Delivery { get; set; }
        }

        public class PostModel
        {
            public ChatroomModel Chatroom { get; set; }

            public MessageModel Message { get; set; }
        }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }
        }
    }
}
