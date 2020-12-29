using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Users;
using ChatJS.Models.Chatrooms;
using ChatJS.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class ChatroomModelBuilder : IChatroomModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly IUserModelBuilder _userBuilder;
        private readonly ApplicationDbContext _dbContext;

        public ChatroomModelBuilder(
            ICacheManager cacheManager,
            IUserModelBuilder userBuilder,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userBuilder = userBuilder;
            _cacheManager = cacheManager;
        }

        public Task<List<ChatroomReadDto>> BuildAllAsync(Guid userId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Memberships(userId), async () =>
            {
                var chatrooms = await _dbContext.Memberships
                    .Where(x => x.UserId == userId)
                    .Where(x => x.Status == MembershipStatusType.Active)
                    .Include(x => x.Chatroom).Select(x => x.Chatroom).ToListAsync();

                var chatroomReadDtos = new List<ChatroomReadDto>();
                foreach (var chatroom in chatrooms)
                {
                    chatroomReadDtos.Add(await _cacheManager.GetOrSetAsync(
                        CacheKeyCollection.Membership(userId, chatroom.Id), () =>
                    {
                        return CreateAsync(userId, chatroom);
                    }));
                }

                return chatroomReadDtos;
            });
        }

        public Task<ChatroomReadDto> BuildAsync(Guid userId, Guid chatroomId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Membership(userId, chatroomId), async () =>
            {
                var chatroom = await _dbContext.Memberships
                    .Where(x => x.UserId == userId)
                    .Where(x => x.ChatroomId == chatroomId)
                    .Where(x => x.Status == MembershipStatusType.Active)
                    .Include(x => x.Chatroom).Select(x => x.Chatroom).FirstOrDefaultAsync();

                return await CreateAsync(userId, chatroom);
            });
        }

        private async Task<ChatroomReadDto> CreateAsync(Guid userId, Chatroom chatroom)
        {
            var chatroomReadDto = new ChatroomReadDto
            {
                Id = chatroom.Id,
                Name = chatroom.Name,
                NameCaption = chatroom.NameCaption
            };

            var members = await GetMembersAsync(chatroom.Id);
            var membersWithoutSelf = members.Where(x => x.Id != userId).ToList();

            if (chatroomReadDto.Name == null ||
                chatroomReadDto.NameCaption == null )
            {
                chatroomReadDto.Name ??= GenerateName(membersWithoutSelf);
                chatroomReadDto.NameCaption ??= GenerateNameCaption(membersWithoutSelf);
            }

            foreach (var member in members)
            {
                chatroomReadDto.Members.Add(_cacheManager.GetOrSet(
                    CacheKeyCollection.User(member.Id), () => new UserReadDto
                {
                    Id = member.Id,
                    Name = member.DisplayName,
                    NameCaption = member.DisplayNameUid,
                }));
            }

            return chatroomReadDto;
        }

        private Task<List<User>> GetMembersAsync(Guid chatroomId)
        {
            return _dbContext.Memberships
                .Where(x => x.ChatroomId == chatroomId)
                .Where(x => x.Status == MembershipStatusType.Active)
                .Include(x => x.User).Select(x => x.User).ToListAsync();
        }

        private static string GenerateName(IList<User> membersWithoutSelf)
        {
            return membersWithoutSelf.Count != 1
                ? string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName).Append("You"))
                : string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName));
        }

        private static string GenerateNameCaption(IList<User> membersWithoutSelf)
        {
            return membersWithoutSelf.Count == 1
                ? $"{membersWithoutSelf.First().DisplayNameUid}"
                : $"{membersWithoutSelf.Count + 1} Members";
        }
    }
}
