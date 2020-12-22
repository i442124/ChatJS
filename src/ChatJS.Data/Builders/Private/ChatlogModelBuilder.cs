using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;
using ChatJS.Models;
using ChatJS.Models.Chatlog;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ChatJS.Data.Builders.Private
{
    public class ChatlogModelBuilder : IChatlogModelBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatlogModelBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatlogAreaModel> BuildAreaAsync(Guid userId)
        {
            var memberships = await _dbContext.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Where(x => x.UserId == userId)

                    .Include(x => x.User)
                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Messages)

                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .ToListAsync();

            var entries = new List<ChatlogEntryModel>();
            foreach (var membership in memberships)
            {
                var chatlogEntryModel = BuildEntry(membership);
                entries.Add(chatlogEntryModel);
            }

            return new ChatlogAreaModel { Entries = entries };
        }

        public async Task<ChatlogEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId)
        {
            var membership = await _dbContext.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Where(x => x.ChatlogId == chatlogId)
                .Where(x => x.UserId == userId)

                    .Include(x => x.User)
                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Messages)

                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Memberships)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync();

            return BuildEntry(membership);
        }

        private ChatlogEntryModel BuildEntry(Membership membership)
        {
            var membersWithoutSelf = GetMembersWithoutSelf(membership);
            if (membersWithoutSelf.Count > 0)
            {
                var chatlogEntryModel = new ChatlogEntryModel();

                var message = GetLatestMessage(membership.Chatlog);
                if (message != null)
                {
                    chatlogEntryModel.Caption = message.Content;
                    chatlogEntryModel.TimeStamp = message.CreatedAt;
                }

                chatlogEntryModel.Name = membersWithoutSelf.Count > 1
                    ? string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName).Append("You"))
                    : string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName));

                return chatlogEntryModel;
            }

            return null;
        }

        private static IList<User> GetMembers(Chatlog chatlog)
        {
            return chatlog.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Select(x => x.User).ToList();
        }

        private static IList<User> GetMembersWithoutSelf(Membership membership)
        {
            return membership.Chatlog.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Where(x => x.UserId != membership.UserId)
                .Select(x => x.User).ToList();
        }

        private static Message GetLatestMessage(Chatlog chatlog)
        {
            return chatlog.Messages.LastOrDefault(m => m.Status != MessageStatusType.Deleted);
        }
    }
}
