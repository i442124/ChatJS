using System;

namespace ChatJS.Domain.Chatrooms.Commands
{
    public class DeleteChatroom
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
