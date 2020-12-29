using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class ReinstateMembership
    {
        public Guid ChatroomId { get; set; }

        public Guid UserId { get; set; }
    }
}
