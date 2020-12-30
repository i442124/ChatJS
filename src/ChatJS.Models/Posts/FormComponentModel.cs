using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatJS.Models.Posts
{
    public class FormComponentModel
    {
        public MessageModel Message { get; set; }

        public ChatroomModel Chatroom { get; set; }

        public class ChatroomModel
        {
            [Required]
            public Guid Id { get; set; }
        }

        public class MessageModel
        {
            [Required]
            public Guid Id { get; set; }

            [Required]
            [StringLength(50)]
            public string Content { get; set; }

            public byte[] Attachment { get; set; }
        }
    }
}
