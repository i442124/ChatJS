﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using ChatJS.Data;
using ChatJS.Models;

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

        public async Task<CurrentUserModel> CurrentUserAsync()
        {
            var result = new CurrentUserModel();
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
                        result.Id = user.Id;
                        result.IsAuthenticated = true;
                        result.DisplayName = user.DisplayName;
                        result.DisplayNameUid = user.DisplayNameUid;
                    }
                }
            }

            return result;
        }
    }
}
