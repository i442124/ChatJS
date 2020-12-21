using System;

namespace ChatJS.Domain.Messages.Commands
{
    public class DeleteMessage
    {
        public int Index { get; set; }

        public Guid ChatlogId { get; set; }
    }
}
