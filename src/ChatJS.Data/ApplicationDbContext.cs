using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
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
