using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Domain.Users.Commands
{
    public class UpdateUser
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }

        public IList<string> Roles { get; set; } = null;
    }
}
