using System;
using System.Collections;
using System.Collections.Generic;

namespace ChatJS.Models.Messages
{
    public class MessageAreaModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string Caption { get; set; }

        public IList<MessageEntryModel> Entries { get; set; }
    }
}
