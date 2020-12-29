using ChatJS.Domain;
using ChatJS.Domain.Posts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder
                .HasOne(x => x.Message)
                .WithMany(navigationName: null)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder
                .HasOne(x => x.Chatroom)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.ChatroomId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
