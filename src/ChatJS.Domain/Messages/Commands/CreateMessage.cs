using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class CreateMessage
    {
        public Guid ChatId { get; set; }

        public string Contents { get; set; }
    }
}
