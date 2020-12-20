using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Messages
{
    public class Message
    {
        public Guid Id { get; set; }

        public User Owner { get; set; }

        public string Contents { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
