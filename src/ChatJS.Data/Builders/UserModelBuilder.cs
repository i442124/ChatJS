using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Data.Caching;
using ChatJS.Domain.Users;
using ChatJS.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders
{
    public class UserModelBuilder : IUserModelBuilder
    {
        private readonly ICacheManager _cacheManager;
        private readonly ApplicationDbContext _dbContext;

        public UserModelBuilder(
            ICacheManager cacheManager,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public Task<UserPageModel> BuildUserPageModelAsync()
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Users(Guid.Empty), async () =>
            {
                var users = await _dbContext.Users
                    .Where(x => x.Status == UserStatusType.Active)
                    .ToListAsync();

                return new UserPageModel
                {
                    Users = users.Select(user =>
                    _cacheManager.GetOrSet(CacheKeyCollection.User(user.Id), () =>
                    {
                        return new UserPageModel.UserModel
                        {
                            Id = user.Id,
                            Name = user.DisplayName,
                            NameUid = user.DisplayNameUid,
                            NameCaption = user.DisplayNameUid
                        };
                    })).ToList()
                };
            });
        }

        public Task<UserPageModel> BuildUserPageModelAsync(Guid userId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.Users(userId), async () =>
            {
                var users = await _dbContext.Users
                    .Where(x => x.Id != userId)
                    .Where(x => x.Status == UserStatusType.Active)
                    .ToListAsync();

                return new UserPageModel
                {
                    Users = users.Select(user =>
                    _cacheManager.GetOrSet(CacheKeyCollection.User(user.Id), () =>
                    {
                        return new UserPageModel.UserModel
                        {
                            Id = user.Id,
                            Name = user.DisplayName,
                            NameUid = user.DisplayNameUid,
                            NameCaption = user.DisplayNameUid
                        };
                    })).ToList()
                };
            });
        }

        public Task<UserPageModel.UserModel> BuildUserModelAsync(Guid userId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.User(userId), async () =>
            {
                var user = await _dbContext.Users
                    .Where(x => x.Id == userId)
                    .Where(x => x.Status == UserStatusType.Active)
                    .FirstOrDefaultAsync();

                return new UserPageModel.UserModel
                {
                    Id = user.Id,
                    Name = user.DisplayName,
                    NameUid = user.DisplayNameUid,
                    NameCaption = user.DisplayNameUid
                };
            });
        }
    }
}
