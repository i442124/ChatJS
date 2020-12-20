using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class UpdateMessage
    {
        public Guid MessageId { get; set; }

        public string Contents { get; set; }
    }
}
