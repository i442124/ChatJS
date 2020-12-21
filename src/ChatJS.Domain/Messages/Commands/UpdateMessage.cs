using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class UpdateMessage
    {
        public Guid ChatlogId { get; set; }

        public string Content { get; set; }

        public int Index { get; set; }

        public Guid UserId { get; set; }
    }
}
