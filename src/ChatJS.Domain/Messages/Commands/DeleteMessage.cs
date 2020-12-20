using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class DeleteMessage
    {
        public Guid MessageId { get; set; }
    }
}
