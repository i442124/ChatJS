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
using ChatJS.Models.Messages;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ChatJS.Data.Builders.Private
{
    public class MessageModelBuilder : IMessageModelBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageModelBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MessageAreaModel> BuildAreaAsync(Guid userId, Guid chatlogId)
        {
            var membership = await _dbContext.Memberships
                .Where(x => x.Status == MembershipStatusType.Active)
                .Where(x => x.ChatlogId == chatlogId)
                .Where(x => x.UserId == userId)

                    .Include(x => x.User)
                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Memberships)

                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Messages)
                    .ThenInclude(x => x.CreatedByUser)
                    .FirstOrDefaultAsync();

            if (membership != null)
            {
                var membersWithoutSelf = GetMembersWithoutSelf(membership);
                if (membersWithoutSelf.Count > 1)
                {
                    var messageArea = BuildGroupArea(membership, membersWithoutSelf);
                    return messageArea;
                }
                else
                {
                    var messageArea = BuildPrivateArea(membership, membersWithoutSelf.First());
                    return messageArea;
                }
            }

            return null;
        }

        public async Task<MessageEntryModel> BuildEntryAsync(Guid userId, Guid chatlogId, int index)
        {
            var message = await _dbContext.Messages
                .Where(x => x.Status == MessageStatusType.Published)
                .Where(x => x.ChatlogId == chatlogId)
                .Where(x => x.Index == index)

                    .Include(x => x.Chatlog)
                    .ThenInclude(x => x.Memberships)
                    .ThenInclude(x => x.User)

                    .Include(x => x.CreatedByUser)
                    .FirstOrDefaultAsync();

            var user = message.Chatlog.Memberships
                .Where(x => x.UserId == userId)
                .Select(x => x.User)
                .FirstOrDefault();

            if (user != null)
            {
                if (message.Chatlog.Memberships.Count > 2)
                {
                    return BuildEntry(message, user);
                }
                else
                {
                    return BuildAnnonymousEntry(message, user);
                }
            }

            return null;
        }

        private MessageAreaModel BuildGroupArea(Membership membership, IList<User> membersWithoutSelf)
        {
            var messageAreaModel = new MessageAreaModel
            {
                Id = membership.ChatlogId,
                Name = string.Join(", ", membersWithoutSelf.Select(x => x.DisplayName).Append("You")),
                Caption = string.Format("{0} Members", membersWithoutSelf.Count + 1),
                Entries = new List<MessageEntryModel>()
            };

            foreach (var message in membership.Chatlog.Messages)
            {
                if (message.Status == MessageStatusType.Published)
                {
                    var messageEntry = BuildEntry(message, membership.User);
                    messageAreaModel.Entries.Add(messageEntry);
                }
            }

            return messageAreaModel;
        }

        private MessageAreaModel BuildPrivateArea(Membership membership, User member)
        {
            var messageAreaModel = new MessageAreaModel
            {
                Id = membership.ChatlogId,
                Name = member.DisplayName,
                Caption = $"#{member.DisplayNameUid}",
                Entries = new List<MessageEntryModel>()
            };

            foreach (var message in membership.Chatlog.Messages)
            {
                if (message.Status == MessageStatusType.Published)
                {
                    var messageEntry = BuildAnnonymousEntry(message, membership.User);
                    messageAreaModel.Entries.Add(messageEntry);
                }
            }

            return messageAreaModel;
        }

        private MessageEntryModel BuildEntry(Message message, User messageContext)
        {
            return new MessageEntryModel
            {
                Index = message.Index,
                Content = message.Content,
                TimeStamp = message.CreatedAt,
                Name = message.CreatedByUser.DisplayName,
                Origin = GetMessageOrigin(message, messageContext)
            };
        }

        private MessageEntryModel BuildAnnonymousEntry(Message message, User messageContext)
        {
            return new MessageEntryModel
            {
                Index = message.Index,
                Content = message.Content,
                TimeStamp = message.CreatedAt,
                Origin = GetMessageOrigin(message, messageContext)
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

        private static MessageOrigin GetMessageOrigin(Message message, User messageContext)
        {
            return message.CreatedBy == messageContext.Id ? MessageOrigin.Send : MessageOrigin.Received;
        }
    }
}
