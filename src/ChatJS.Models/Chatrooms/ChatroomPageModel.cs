using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Chatrooms
{
    public class ChatroomPageModel
    {
        public List<ChatroomModel> Chatrooms { get; set; }

        public class ChatroomModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameCaption { get; set; }

            public ChatroomStatus Status { get; set; }

            public List<UserModel> Members { get; set; }
        }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }

            public string NameCaption { get; set; }
        }
    }
}
