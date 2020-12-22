using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class UpdateMembership
    {
        public MembershipStatusType Status { get; set; }

        public Guid ChatlogId { get; set; }

        public Guid UserId { get; set; }
    }
}
