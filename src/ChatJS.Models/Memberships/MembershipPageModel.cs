using System;
using System.Collections.Generic;

using ChatJS.Models.Chatrooms;

namespace ChatJS.Models.Memberships
{
    public class MembershipPageModel
    {
        public List<ChatroomModel> Chatrooms { get; set; }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }

            public string NameCaption { get; set; }
        }

        public class MembershipModel
        {
            public Guid Id { get; set; }

            public List<UserModel> Members { get; set; }
        }

        public class ChatroomModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameCaption { get; set; }

            public List<UserModel> Members { get; set; }

            public ChatroomModelStatus Status { get; set; }
        }
    }
}
