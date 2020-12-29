using System;

namespace ChatJS.Domain.Posts.Commands
{
    public class GetPostByIds
    {
        public Guid MessageId { get; set; }

        public Guid ChatroomId { get; set; }
    }
}
