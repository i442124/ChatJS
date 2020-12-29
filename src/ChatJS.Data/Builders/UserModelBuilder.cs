using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data.Caching;
using ChatJS.Domain.Users;
using ChatJS.Models;
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

        public Task<UserReadDto> BuildAsync(Guid userId)
        {
            return _cacheManager.GetOrSetAsync(CacheKeyCollection.User(userId), async () =>
            {
                var user = await _dbContext.Users
                    .Where(x => x.Id == userId)
                    .Where(x => x.Status == UserStatusType.Active)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return null;
                }

                return new UserReadDto
                {
                    Id = user.Id,
                    Name = user.DisplayName,
                    NameCaption = user.DisplayNameUid,
                };
            });
        }
    }
}
