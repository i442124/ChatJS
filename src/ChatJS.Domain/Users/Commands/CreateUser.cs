using System;

namespace ChatJS.Domain.Users.Commands
{
    public class CreateUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string IdentityUserId { get; set; }

        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }
    }
}
