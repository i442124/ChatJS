using System;

namespace ChatJS.Data.Caching
{
    public class CacheKeyCollection
    {
        public static string Chatroom(Guid userId, Guid chatroomId)
        => $"Chatroom | UserId: {userId}, ChatroomId: {chatroomId}";

        public static string Chatrooms(Guid userId)
         => $"Chatrooms | UserId: {userId}";

        public static string Chatlog(Guid userId, Guid chatroomId)
        => $"Chatlog | UserId: {userId}, ChatroomId: {chatroomId}";

        public static string ChatlogEntry(Guid userId, Guid chatroomId, Guid messageId)
        => $"ChatlogEntry | UserId: {userId}, ChatroomId: {chatroomId} MessageId: {messageId}";

        public static string Message(Guid messageId)
        => $"Message | MessageId: {messageId}";

        public static string User(Guid userId)
        => $"User | UserId: {userId}";
    }
}
