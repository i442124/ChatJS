using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;

namespace ChatJS.Domain.Chatrooms
{
    public class Chatroom
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameCaption { get; set; }

        public ChatroomStatusType Status { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
