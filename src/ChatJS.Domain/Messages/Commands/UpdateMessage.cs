using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class UpdateMessage
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public byte[] Attachment { get; set; }
    }
}
