using System;
using System.Collections;
using System.Collections.Generic;

using ChatJS.Models.Users;

namespace ChatJS.Models.Chatrooms
{
    public class ChatroomReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameCaption { get; set; }

        public ChatroomStatus Status { get; set; }

        public List<UserReadDto> Members { get; set; }
        = new List<UserReadDto>();
    }
}
