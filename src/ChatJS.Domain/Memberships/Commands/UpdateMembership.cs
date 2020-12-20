using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class UpdateMembership
    {
        public Guid ChatlogId { get; set; }

        public Guid UserId { get; set; }
    }
}
