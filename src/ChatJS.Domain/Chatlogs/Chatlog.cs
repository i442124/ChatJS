using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Chatlogs
{
    public class Chatlog
    {
        public Guid Id { get; set; }

        public virtual ICollection<User> Members { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
