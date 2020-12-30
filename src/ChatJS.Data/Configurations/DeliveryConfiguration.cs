using System;

using ChatJS.Domain;
using ChatJS.Domain.Deliveries;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.ToTable("Deliveries");

            builder
                .Property(x => x.ReadAt)
                .HasConversion(x => x, x =>
                    DateTime.SpecifyKind(x, DateTimeKind.Utc));

            builder
                .Property(x => x.ReceivedAt)
                .HasConversion(x => x, x =>
                    DateTime.SpecifyKind(x, DateTimeKind.Utc));

            builder
                .HasOne(x => x.Message)
                .WithMany(x => x.Deliveries)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany(navigationName: null)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
