using System;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Domain;
using ChatJS.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Rules
{
    public class UserRules : IUserRules
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRules(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsValidAsync(Guid id)
        {
            var any = await _dbContext.Users
                .AnyAsync(user =>
                    user.Id == id &&
                    user.Status == UserStatusType.Active);

            return any;
        }

        public async Task<bool> IsDisplayNameUniqueAsync(string displayName)
        {
            var any = await _dbContext.Users
                .AnyAsync(user =>
                    user.DisplayName == displayName &&
                    user.Status != UserStatusType.Deleted);

            return !any;
        }

        public async Task<bool> IsDisplayNameUniqueAsync(string displayName, string displayNameUid)
        {
            var any = await _dbContext.Users
                .AnyAsync(user =>
                    user.DisplayName == displayName &&
                    user.DisplayNameUid == displayNameUid &&
                    user.Status != UserStatusType.Deleted);

            return !any;
        }
    }
}
