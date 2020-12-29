using System;

namespace ChatJS.Domain.Chatrooms.Commands
{
    public class CreateChatroom
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string NameCaption { get; set; }
    }
}
