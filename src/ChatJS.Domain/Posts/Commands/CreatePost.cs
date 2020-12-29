using System;

namespace ChatJS.Domain.Posts.Commands
{
    public class CreatePost
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid MessageId { get; set; }

        public Guid ChatroomId { get; set; }
    }
}
