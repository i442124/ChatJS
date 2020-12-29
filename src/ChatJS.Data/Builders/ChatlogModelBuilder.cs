using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Chatlogs;
using ChatJS.Models.Chatrooms;
using ChatJS.Models.Messages;

namespace ChatJS.Data.Builders
{
    public class ChatlogModelBuilder : IChatlogModelBuilder
    {
        private readonly IMessageModelBuilder _messageBuilder;
        private readonly IChatroomModelBuilder _chatroomBuilder;

        public ChatlogModelBuilder(
            IMessageModelBuilder messageBuilder,
            IChatroomModelBuilder chatroomBuilder)
        {
            _messageBuilder = messageBuilder;
            _chatroomBuilder = chatroomBuilder;
        }

        public async Task<ChatlogReadDto> BuildAsync(Guid userId, Guid chatroomId)
        {
            var chatroom = await _chatroomBuilder.BuildAsync(userId, chatroomId);
            var chatroomMessages = await _messageBuilder.BuildAllAsync(userId, chatroomId);
            return new ChatlogReadDto { Chatroom = chatroom, Messages = chatroomMessages };
        }
    }
}
