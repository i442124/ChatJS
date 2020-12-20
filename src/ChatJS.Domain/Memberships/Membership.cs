using System;

using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Memberships
{
    public class Membership
    {
        public Chatlog Chatlog { get; set; }

        public Guid ChatlogId { get; set; }

        public User User { get; set; }

        public Guid UserId { get; set; }

        public MembershipStatusType Status { get; set; }
    }
}
