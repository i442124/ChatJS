using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Users
{
    public class UserPageModel
    {
        public List<UserModel> Users { get; set; }

        public class UserModel
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string NameUid { get; set; }

            public string NameCaption { get; set; }

            public UserModelStatus Status { get; set; }
        }

        public class ChatroomModel
        {
            public Guid Id { get; set; }
        }
    }
}
