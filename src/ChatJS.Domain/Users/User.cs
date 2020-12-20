using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;

namespace ChatJS.Domain.Users
{
    public class User
    {
        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }

        public string IdentityUserId { get; set; }

        public Guid Id { get; set; }

        public virtual ICollection<Membership> Memberships { get; set; }

        public UserStatusType Status { get; set; }
    }
}
