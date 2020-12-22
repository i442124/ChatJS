using System;

namespace ChatJS.Models.Messages
{
    public class MessageEntryModel
    {
        public int Index { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime TimeStamp { get; set; }

        public MessageOrigin Origin { get; set; }
    }
}
