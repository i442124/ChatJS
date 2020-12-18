﻿using System;
using System.Reflection;

using IdentityServer4.EntityFramework.Options;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ChatJS.WebServer.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(
                assembly: Assembly.GetExecutingAssembly());
        }
    }
}
