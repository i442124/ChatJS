using System;

namespace ChatJS.Domain.Chatrooms.Commands
{
    public class UpdateChatroom
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameCaption { get; set; }
    }
}
