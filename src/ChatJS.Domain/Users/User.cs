using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;

namespace ChatJS.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public virtual ICollection<Chatlog> Chatlogs { get; set; }
    }
}
