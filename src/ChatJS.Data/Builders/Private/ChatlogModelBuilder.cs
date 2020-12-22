using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;
using ChatJS.Models.Chatlog;

namespace ChatJS.Data.Builders.Private
{
    public class ChatlogModelBuilder : IChatlogModelBuilder
    {
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        public ChatlogModelBuilder(
            IUserService userService,
            IMembershipService membershipService)
        {
            _userService = userService;
            _membershipService = membershipService;
        }

        public async Task<ChatlogAreaModel> BuildAreaAsync(Guid userId)
        {
            var userById = new GetUserById { Id = userId };
            var user = await _userService.GetByIdAsync(userById);

            var entries = new List<ChatlogEntryModel>();
            foreach (var membership in user.Memberships)
            {
                if (membership.Status == MembershipStatusType.Active)
                {
                    var entryModel = BuildChatlogEntry(membership);
                    entries.Add(entryModel);
                }
            }

            return new ChatlogAreaModel { Entries = entries };
        }

        public async Task<ChatlogEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId)
        {
            var membershipById = new GetMembershipById { UserId = userId, ChatlogId = chatlogId };
            var membership = await _membershipService.GetByIdAsync(membershipById);
            return BuildChatlogEntry(membership);
        }

        private ChatlogEntryModel BuildChatlogEntry(Membership membership)
        {
            var chatlogEntryModel = new ChatlogEntryModel();
            var membersWithoutSelf = GetMembersWithoutSelf(membership);
            if (membersWithoutSelf.Count > 0)
            {
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
