using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Data.Caching;
using ChatJS.Domain.Chatrooms;
using ChatJS.Models.Chatrooms;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class ChatroomModelBuilder : IChatroomModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public ChatroomModelBuilder(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public Task<ChatroomPageModel> BuildChatroomPageModelAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatroom(chatroomId), async () =>
            {
                var chatroom = await _dbContext.Chatrooms
                   .Where(x => x.Id == chatroomId)
                   .Where(x => x.Status == ChatroomStatusType.Active)
                   .Include(x => x.Memberships)
                   .ThenInclude(x => x.User)
                   .FirstOrDefaultAsync();

                return new ChatroomPageModel
                {
                    Chatroom = new ChatroomPageModel.ChatroomModel
                    {
                        Id = chatroom.Id,
                        Name = chatroom.Name,
                        NameCaption = chatroom.NameCaption,
                    },

                    Members = chatroom.Memberships
                        .Select(x => x.User)
                        .Select(x => new ChatroomPageModel.UserModel
                        {
                            Id = x.Id,
                            Name = x.DisplayName,
                            NameUid = x.DisplayNameUid,
                            NameCaption = x.DisplayNameUid
                        }).ToList()
                };
            });
        }

        public Task<ChatroomPageModel.ChatroomModel> BuildChatroomModelAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatroom(chatroomId), async () =>
            {
                var chatroom = await _dbContext.Chatrooms
                   .Where(x => x.Id == chatroomId)
                   .Where(x => x.Status == ChatroomStatusType.Active)
                   .FirstOrDefaultAsync();

                return new ChatroomPageModel.ChatroomModel
                {
                    Id = chatroom.Id,
                    Name = chatroom.Name,
                    NameCaption = chatroom.NameCaption,
                };
            });
        }
    }
}
