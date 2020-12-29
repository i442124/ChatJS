using System.Collections;
using System.Collections.Generic;

using ChatJS.Models.Chatrooms;
using ChatJS.Models.Messages;

namespace ChatJS.Models.Chatlogs
{
    public class ChatlogReadDto
    {
        public ChatroomReadDto Chatroom { get; set; }

        public List<MessageReadDto> Messages { get; set; }
    }
}
