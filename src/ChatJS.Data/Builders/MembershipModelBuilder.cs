using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Data.Caching;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Models.Memberships;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class MembershipModelBuilder : IMembershipModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public MembershipModelBuilder(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public Task<MembershipPageModel> BuildMembershipPageModelAsync(Guid userId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Memberships(userId), async () =>
            {
                var chatrooms = await _dbContext.Memberships
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Status == MembershipStatusType.Active)
                        .Include(x => x.Chatroom)
                        .ThenInclude(x => x.Memberships)
                        .ThenInclude(x => x.User)
                        .Select(x => x.Chatroom)
                        .ToListAsync();

                return new MembershipPageModel
                {
                    Chatrooms = chatrooms.Select(chatroom =>
                    _cacheManager.GetOrSet(CacheKeyCollection.Members(chatroom.Id), () =>
                    {
                        return new MembershipPageModel.ChatroomModel
                        {
                            Id = chatroom.Id,
                            Name = chatroom.Name,
                            NameCaption = chatroom.NameCaption,
                            Members = GetMembers(chatroom)
                        };
                    })).ToList()
                };
            });
        }

        public Task<MembershipPageModel.ChatroomModel> BuildMembershipModelAsync(Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Members(chatroomId), async () =>
            {
                var chatroom = await _dbContext.Chatrooms
                    .Where(x => x.Id == chatroomId)
                    .Where(x => x.Status == ChatroomStatusType.Active)
                    .Include(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync();

                return new MembershipPageModel.ChatroomModel
                {
                    Id = chatroomId,
                    Name = chatroom.Name,
                    NameCaption = chatroom.NameCaption,
                    Members = GetMembers(chatroom)
                };
            });
        }

        private static List<MembershipPageModel.UserModel> GetMembers(Chatroom chatroom)
        {
            return chatroom.Memberships
                .Select(x => x.User)
                .Select(x =>
                {
                    return new MembershipPageModel.UserModel
                    {
                        Id = x.Id,
                        Name = x.DisplayName,
                        NameUid = x.DisplayNameUid,
                        NameCaption = x.DisplayNameUid
                    };
                }).ToList();
        }
    }
}
