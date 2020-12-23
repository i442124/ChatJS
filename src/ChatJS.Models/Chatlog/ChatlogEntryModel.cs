using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Chatlog
{
    public class ChatlogEntryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Caption { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
