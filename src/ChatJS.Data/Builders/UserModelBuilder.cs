using System;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Data.Caching;
using ChatJS.Domain.Memberships;
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

        public async Task<UserPageModel> BuildUserPageModelAsync(Guid userId)
        {
            return await _cacheManager.GetOrSetAsync(CacheKeyCollection.User(userId), async () =>
            {
                var user = await _dbContext.Users
                    .Where(x => x.Id == userId)
                    .Where(x => x.Status == UserStatusType.Active)
                        .Include(x => x.Memberships)
                        .FirstOrDefaultAsync();

                return new UserPageModel
                {
                    Id = user.Id,
                    Name = user.DisplayName,
                    NameUid = user.DisplayNameUid,
                    NameCaption = user.DisplayNameUid,

                    Chatrooms = user.Memberships
                        .Where(x => x.Status == MembershipStatusType.Active)
                        .Select(x => new UserPageModel.ChatroomModel { Id = x.ChatroomId })
                        .ToList()
                };
            });
        }

        public async Task<UserPageModel.UserModel> BuildUserModelAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .Where(x => x.Id == userId)
                .Where(x => x.Status == UserStatusType.Active)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            return new UserPageModel.UserModel
            {
                Id = user.Id,
                Name = user.DisplayName,
                NameUid = user.DisplayNameUid,
                NameCaption = user.DisplayNameUid
            };
        }
    }
}
