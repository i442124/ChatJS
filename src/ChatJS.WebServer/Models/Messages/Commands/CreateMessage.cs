using System;

namespace ChatJS.WebServer.Models.Messages.Commands
{
    public class CreateMessage
    {
        public Guid ChatId { get; set; }

        public string Contents { get; set; }
    }
}
