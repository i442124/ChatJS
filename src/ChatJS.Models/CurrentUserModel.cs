using System;

namespace ChatJS.Models
{
    public class CurrentUserModel
    {
        public Guid Id { get; set; }

        public bool IsAuthenticated { get; set; }

        public string DisplayName { get; set; }

        public string DisplayNameUid { get; set; }
    }
}
