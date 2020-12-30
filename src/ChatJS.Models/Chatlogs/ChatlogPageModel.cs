using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Models;
using ChatJS.Models.Messages;

namespace ChatJS.Models.Chatlogs
{
    public class ChatlogPageModel
    {
        public List<MessageModel> Messages { get; set; }

        public class MessageModel
        {
            public Guid Id { get; set; }

            public string Content { get; set; }

            public UserModel Creator { get; set; }

            public DateTime TimeStamp { get; set; }

            public DeliveryModel Delivery { get; set; }
        }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }
        }

        public class DeliveryModel
        {
            public bool WasReadByEveryone { get; set; }

            public bool WasReceivedByEveryone { get; set; }
        }
    }
}
