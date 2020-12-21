using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatJS.WebServer.Services
{
    public class IntegrityService : IIntegrityService
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _dbContext;

        public IntegrityService(
            IUserService userService,
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task EnsureUserCreatedAsync(IdentityUser identityUser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdentityUserId == identityUser.Id);
            if (user == null)
            {
                await _userService.CreateAsync(new CreateUser
                {
                    IdentityUserId = identityUser.Id,
                    DisplayName = identityUser.UserName,
                    DisplayNameUid = "0000",
                });
            }
        }

        public async Task EnsureUserConfirmedAsync(IdentityUser identityUser)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdentityUserId == identityUser.Id);
            if (user != null && user.Status == UserStatusType.Peinding)
            {
                await _userService.ConfirmAsync(new ConfirmUser
                {
                    Id = user.Id
                });
            }
        }
    }
}
