using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;
using ChatJS.Models;
using ChatJS.Models.Messages;

namespace ChatJS.Data.Builders.Private
{
    public class MessageModelBuilder : IMessageModelBuilder
    {
        private IMembershipService _membershipService;

        public async Task<MessageAreaModel> BuildAreaAsync(Guid userId, Guid chatlogId)
        {
            var membershipById = new GetMembershipById { UserId = userId, ChatlogId = chatlogId };
            var membership = await _membershipService.GetByIdAsync(membershipById);

            if (membership != null)
            {
                var membersWithoutSelf = GetMembersWithoutSelf(membership);
                if (membersWithoutSelf.Count > 1)
                {
                    var messageAreaModel = BuildGroupMessageArea(membership, membersWithoutSelf);
                    return messageAreaModel;
                }
                else
                {
                    var messageAreaModel = BuildPrivateMessageAreaModel(membership, membersWithoutSelf.First());
                    return messageAreaModel;
                }
            }

            return null;
        }

        public async Task<MessageEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId, int index)
        {
            var membershipById = new GetMembershipById { UserId = userId, ChatlogId = chatlogId };
            var membership = await _membershipService.GetByIdAsync(membershipById);

            var messages = membership.Chatlog.Messages.ToList();
            var message = messages[index];

            if (membership != null)
            {
                var membersWithoutSelf = GetMembersWithoutSelf(membership);
                if (membersWithoutSelf.Count > 1)
                {
                    BuildMessageEntry(message, membership.User);
                }
                else
                {
                    return BuildAnnonymousMessageEntry(message, membership.User);
                }
            }

            return null;
        }

        private MessageAreaModel BuildGroupMessageArea(Membership membership, IList<User> membersWithoutSelf)
        {
            var messageAreaModel = new MessageAreaModel
            {
                Id = membership.ChatlogId,
                Name = string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName).Append("You")),
                Caption = string.Format("{0} Members", membersWithoutSelf.Count + 1)
            };

            foreach (var message in membership.Chatlog.Messages)
            {
                var messageEntry = BuildMessageEntry(message, membership.User);
                messageAreaModel.Entries.Add(messageEntry);
            }

            return messageAreaModel;
        }

        private MessageAreaModel BuildPrivateMessageAreaModel(Membership membership, User member)
        {
            var messageAreaModel = new MessageAreaModel
            {
                Id = membership.ChatlogId,
                Name = member.DisplayName,
                Caption = member.DisplayNameUid
            };

            foreach (var message in membership.Chatlog.Messages)
            {
                var messageEntry = BuildAnnonymousMessageEntry(message, membership.User);
                messageAreaModel.Entries.Add(messageEntry);
            }

            return messageAreaModel;
        }

        private MessageEntryModel BuildMessageEntry(Message message, User context)
        {
            return new MessageEntryModel
            {
                Index = message.Index,
                Content = message.Content,
                TimeStamp = message.CreatedAt,
                Name = message.CreatedByUser.DisplayName,
                Origin = GetMessageOrigin(message, context)
            };
        }

        private MessageEntryModel BuildAnnonymousMessageEntry(Message message, User context)
        {
            return new MessageEntryModel
            {
                Index = message.Index,
                Content = message.Content,
                TimeStamp = message.CreatedAt,
                Origin = GetMessageOrigin(message, context)
            };
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

        private static MessageOriginAttribute GetMessageOrigin(Message message, User userContext)
        {
            return message.CreatedBy == userContext.Id ? MessageOriginAttribute.Send : MessageOriginAttribute.Received;
        }
    }
}
