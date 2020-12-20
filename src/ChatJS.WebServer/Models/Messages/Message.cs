using System;

namespace ChatJS.WebServer.Models.Messages
{
    public class Message
    {
        public Guid ChatId { get; set; }

        public Guid MessageId { get; set; }

        public string Contents { get; set; }
    }
}
