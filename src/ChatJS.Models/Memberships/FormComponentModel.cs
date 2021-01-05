using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatJS.Models.Memberships
{
    public class FormComponentModel
    {
        public UserModel User { get; set; }

        public ChatroomModel Chatroom { get; set; }

        public class UserModel
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public string NameUid { get; set; }
        }

        public class ChatroomModel
        {
            [Required]
            public Guid Id { get; set; }
        }
    }
}
