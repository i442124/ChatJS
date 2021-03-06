﻿using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Memberships;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class MembershipRules : IMembershipRules
    {
        private readonly ApplicationDbContext _dbContext;

        public MembershipRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid userId, Guid chatroomId)
        {
            var any = await _dbContext.Memberships
                .AnyAsync(membership =>
                    membership.UserId == userId &&
                    membership.ChatroomId == chatroomId &&
                    membership.Status == MembershipStatusType.Active);

            return any;
        }
    }
}
