using System;
using System.Reflection;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Deliveries;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace ChatJS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(
                assembly: Assembly.GetExecutingAssembly());
        }

        public DbSet<Chatroom> Chatrooms { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }

        public DbSet<Membership> Memberships { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
