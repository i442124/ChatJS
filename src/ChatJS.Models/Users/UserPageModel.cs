using System;
using System.Collections.Generic;

namespace ChatJS.Models.Users
{
    public class UserPageModel
    {
        public string Name { get; set; }

        public string NameUid { get; set; }

        public string NameCaption { get; set; }

        public List<ChatroomModel> Chatrooms { get; set; }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }

            public string NameCaption { get; set; }
        }

        public class ChatroomModel
        {
            public Guid Id { get; set; }
        }
    }
}
