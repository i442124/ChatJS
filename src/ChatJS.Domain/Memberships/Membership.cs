using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Memberships
{
    public class Membership
    {
        public User User { get; set; }

        public Guid UserId { get; set; }

        public Guid ChatroomId { get; set; }

        public Chatroom Chatroom { get; set; }

        public MembershipStatusType Status { get; set; }
    }
}
