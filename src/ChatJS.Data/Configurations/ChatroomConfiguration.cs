using ChatJS.Domain;
using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Posts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class ChatroomConfiguration : IEntityTypeConfiguration<Chatroom>
    {
        public void Configure(EntityTypeBuilder<Chatroom> builder)
        {
            builder.ToTable("Chatrooms");

            builder
                .HasMany(x => x.Posts)
                .WithOne(x => x.Chatroom)
                .HasForeignKey(x => x.ChatroomId);

            builder
                .HasMany(x => x.Memberships)
                .WithOne(x => x.Chatroom)
                .HasForeignKey(x => x.ChatroomId);
        }
    }
}
