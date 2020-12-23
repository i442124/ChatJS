using System;
using System.Linq;
using System.Threading.Tasks;

using ChatJS.Models;
using ChatJS.Models.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data.Builders.Private
{
    public class UserModelBuilder : IUserModelBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserModelBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserAreaModel> BuildAreaAsync(Guid userId)
        {
            var user = await _dbContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            return new UserAreaModel
            {
                Name = user.DisplayName,
                NameUid = user.DisplayNameUid,
                Caption = $"#{user.DisplayNameUid}"
            };
        }
    }
}
