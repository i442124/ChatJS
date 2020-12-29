using System;

using ChatJS.Domain;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Messages;

namespace ChatJS.Domain.Posts
{
    public class Post
    {
        public Guid Id { get; set; }

        public Guid MessageId { get; set; }

        public Message Message { get; set; }

        public Guid ChatroomId { get; set; }

        public Chatroom Chatroom { get; set; }

        public PostStatusType Status { get; set; }
    }
}
