using System;

namespace ChatJS.WebServer.Models.Messages.Commands
{
    public class UpdateMessage
    {
        public Guid MessageId { get; set; }

        public string Contents { get; set; }
    }
}
