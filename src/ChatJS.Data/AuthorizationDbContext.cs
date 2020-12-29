using System;
using System.Reflection;

using IdentityServer4.EntityFramework;
using IdentityServer4.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatJS.Data
{
    public class AuthorizationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public AuthorizationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
    }
}
