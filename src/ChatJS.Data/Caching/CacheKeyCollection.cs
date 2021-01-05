using System;

namespace ChatJS.Data.Caching
{
    public class CacheKeyCollection
    {
        public static string Chatroom(Guid chatroomId)
        => $"Chatroom | ChatroomId: {chatroomId}";

        public static string Memberships(Guid userId)
        => $"Memberships | UserId: {userId}";

        public static string Members(Guid chatroomId)
        => $"Members | ChatroomId: {chatroomId}";

        public static string Message(Guid messageId)
        => $"Message | MessageId: {messageId}";

        public static string Posts(Guid chatroomId)
        => $"Posts | ChatroomId: {chatroomId}";

        public static string Post(Guid postId)
        => $"Post | PostId: {postId}";

        public static string Users(Guid userId)
        => $"Users | UserId: {userId}";

        public static string User(Guid userId)
        => $"User | UserId: {userId}";
    }
}
