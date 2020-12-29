using ChatJS.Domain;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(x => x.DisplayName).IsRequired();
            builder.Property(x => x.DisplayNameUid).IsRequired();
            builder.HasIndex(x => new { x.DisplayName, x.DisplayNameUid }).IsUnique();

            builder
                .HasMany(x => x.Messages)
                .WithOne(x => x.CreatedByUser)
                .HasForeignKey(x => x.CreatedBy);

            builder
                .HasMany(x => x.Memberships)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}
