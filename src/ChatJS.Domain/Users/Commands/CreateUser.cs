using System;

namespace ChatJS.Domain.Users.Commands
{
    public class CreateUser
    {
        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }

        public string IdentityUserId { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
