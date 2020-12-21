using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class CreateMessage
    {
        public Guid ChatlogId { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }
    }
}
