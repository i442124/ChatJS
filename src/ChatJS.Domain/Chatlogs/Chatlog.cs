using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Chatlogs
{
    public class Chatlog
    {
        public Guid Id { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public ChatlogStatusType Status { get; set; }
    }
}
