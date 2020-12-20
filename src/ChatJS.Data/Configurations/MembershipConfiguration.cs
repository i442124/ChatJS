using ChatJS.Domain;
using ChatJS.Domain.Memberships;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.ToTable("Memberships");
            builder.HasKey(x => new { x.UserId, x.ChatlogId });

            builder
                .HasOne(x => x.Chatlog)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.ChatlogId);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.UserId);
        }
    }
}
