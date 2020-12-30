using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class CreateMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }
    }
}
