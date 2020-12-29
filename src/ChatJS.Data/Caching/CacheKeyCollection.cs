using System;

namespace ChatJS.Data.Caching
{
    public class CacheKeyCollection
    {
        public static string Delivery(Guid messageId)
        => $"Delivery | MessageId: {messageId}";

        public static string Membership(Guid userId, Guid chatroomId)
        => $"Membership | UserId: {userId}, ChatroomId: {chatroomId}";

        public static string Memberships(Guid userId)
         => $"Memberships | UserId: {userId}";

        public static string Message(Guid userId, Guid messageId)
        => $"Message | UserId: {userId}, MessageId: {messageId}";

        public static string Messages(Guid userId, Guid chatroomId)
        => $"Messages | UserId: {userId}, ChatroomId: {chatroomId}";

        public static string User(Guid userId)
        => $"User | UserId: {userId}";
    }
}
