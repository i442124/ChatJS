using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;

namespace ChatJS.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }

        public string IdentityUserId { get; set; }

        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }

        public UserStatusType Status { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }
    }
}
