using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ChatJS.WebServer.Services
{
    public class ContextService : IContextService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccesstor;

        public ContextService(
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccesstor = httpContextAccessor;
        }

        public async Task<Guid> CurrentUserAsync()
        {
            var claimsPrincipal = _httpContextAccesstor.HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                var identityUserId = _httpContextAccesstor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(identityUserId))
                {
                    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
                    if (user != null)
                    {
                        return user.Id;
                    }
                }
            }

            return default;
        }
    }
}
