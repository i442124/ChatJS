using System;

namespace ChatJS.Domain.Memberships.Commands
{
    public class CreateMembership
    {
        public Guid ChatroomId { get; set; }

        public Guid UserId { get; set; }
    }
}
