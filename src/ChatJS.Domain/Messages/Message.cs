using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Users;

namespace ChatJS.Domain.Messages
{
    public class Message
    {
        public Chatlog Chatlog { get; set; }

        public Guid ChatlogId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public User CreatedByUser { get; set; }

        public Guid CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Index { get; set; }

        public MessageStatusType Status { get; set; }
    }
}
