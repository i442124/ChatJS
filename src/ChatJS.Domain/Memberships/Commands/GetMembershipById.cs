using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class GetMembershipById
    {
        public Guid UserId { get; set; }

        public Guid ChatlogId { get; set; }
    }
}
