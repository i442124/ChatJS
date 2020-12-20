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

        public Task<bool> IsValidAsync(Guid id)
        {
            return _dbContext.Users
                .AnyAsync(user =>
                    user.Id == id &&
                    user.Status != UserStatusType.Deleted);
        }

        public Task<bool> IsDisplayNameUniqueAsync(string displayName)
        {
            return _dbContext.Users
                .AnyAsync(user =>
                    user.DisplayName == displayName &&
                    user.Status != UserStatusType.Deleted);
        }

        public Task<bool> IsDisplayNameUniqueAsync(string displayName, string displayNameUid)
        {
            return _dbContext.Users
                .AnyAsync(user =>
                    user.DisplayName == displayName &&
                    user.DisplayNameUid == displayNameUid &&
                    user.Status != UserStatusType.Deleted);
        }
    }
}
