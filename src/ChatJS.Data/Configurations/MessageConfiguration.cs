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
            builder.HasKey(x => new { x.Index, x.CreatedBy, x.ChatlogId });

            builder
                .HasOne(x => x.CreatedByUser);

            builder
                .HasOne(x => x.Chatlog)
                .WithMany(x => x.Messages);
        }
    }
}
