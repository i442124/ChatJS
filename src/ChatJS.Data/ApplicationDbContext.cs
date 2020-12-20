using System;
using System.Reflection;

using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Users;

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

        public DbSet<Chatlog> Chatlogs { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
