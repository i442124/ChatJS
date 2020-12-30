using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Users;
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

        public async Task<ChatroomPageModel> BuildChatroomPageModelAsync(Guid userId)
        {
            return await _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatrooms(userId), async () =>
            {
                var chatrooms = await _dbContext.Memberships
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Status == MembershipStatusType.Active)
                        .Include(x => x.Chatroom)
                        .ThenInclude(x => x.Memberships)
                        .ThenInclude(x => x.User)
                        .Select(x => x.Chatroom)
                        .ToListAsync();

                return new ChatroomPageModel
                {
                    Chatrooms = chatrooms.Select(chatroom =>
                    {
                        return _cacheManager.GetOrSet(CacheKeyCollection.Chatroom(userId, chatroom.Id), () =>
                        {
                            var members = GetMembers(chatroom);
                            var membersWithoutSelf = members
                                .Where(x => x.Id != userId)
                                .ToList();

                            return new ChatroomPageModel.ChatroomModel
                            {
                                Id = chatroom.Id,
                                Members = members,
                                Name = chatroom.Name ?? GetName(membersWithoutSelf),
                                NameCaption = chatroom.NameCaption ?? GetNameCaption(membersWithoutSelf)
                            };
                        });
                    }).ToList()
                };
            });
        }

        public async Task<ChatroomPageModel.ChatroomModel> BuildChatroomModelAsync(Guid userId, Guid chatroomId)
        {
            return await _cacheManager.GetOrSetAsync(CacheKeyCollection.Chatroom(userId, chatroomId), async () =>
            {
                var chatroom = await _dbContext.Memberships
                    .Where(x => x.UserId == userId)
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == MembershipStatusType.Active)
                        .Include(x => x.Chatroom)
                        .ThenInclude(x => x.Memberships)
                        .ThenInclude(x => x.User)
                        .Select(x => x.Chatroom)
                        .FirstOrDefaultAsync();

                var members = GetMembers(chatroom);
                var membersWithoutSelf = members
                    .Where(x => x.Id != userId)
                    .ToList();

                return new ChatroomPageModel.ChatroomModel
                {
                    Id = chatroom.Id,
                    Members = members,
                    Name = chatroom.Name ?? GetName(membersWithoutSelf),
                    NameCaption = chatroom.NameCaption ?? GetNameCaption(membersWithoutSelf)
                };
            });
        }

        private static List<ChatroomPageModel.UserModel> GetMembers(Chatroom chatroom)
        {
            return chatroom.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Select(x =>
                {
                    return new ChatroomPageModel.UserModel
                    {
                        Id = x.UserId,
                        Name = x.User.DisplayName,
                        NameUid = x.User.DisplayNameUid,
                        NameCaption = x.User.DisplayNameUid
                    };
                }).ToList();
        }

        private static string GetName(List<ChatroomPageModel.UserModel> membersWithoutSelf)
        {
            return membersWithoutSelf.Count != 1
                ? string.Join(", ", membersWithoutSelf.Select(x => x.Name).Append("You"))
                : string.Join(", ", membersWithoutSelf.Select(x => x.Name));
        }

        private static string GetNameCaption(List<ChatroomPageModel.UserModel> membersWithoutSelf)
        {
            return membersWithoutSelf.Count == 1
                ? $"{membersWithoutSelf.First().NameUid}"
                : $"{membersWithoutSelf.Count + 1} Members";
        }
    }
}
