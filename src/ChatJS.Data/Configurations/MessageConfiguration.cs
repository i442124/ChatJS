using System;

using ChatJS.Domain;
using ChatJS.Domain.Messages;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");
            builder.Property(x => x.Content).IsRequired();

            builder
                .Property(x => x.CreatedAt)
                .HasConversion(x => x, x =>
                    DateTime.SpecifyKind(x, DateTimeKind.Utc));

            builder
                .Property(x => x.ModifiedAt)
                .HasConversion(x => x, x => x == null ? null :
                    DateTime.SpecifyKind(x.Value, DateTimeKind.Utc));

            builder
                .HasMany(x => x.Deliveries)
                .WithOne(x => x.Message)
                .HasForeignKey(x => x.MessageId);

            builder
                .HasOne(x => x.CreatedByUser)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
