using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class DeleteMembership
    {
        public Guid ChatlogId { get; set; }

        public Guid UserId { get; set; }
    }
}
