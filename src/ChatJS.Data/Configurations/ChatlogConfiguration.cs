using ChatJS.Domain;
using ChatJS.Domain.Chatlogs;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.Data.Configurations
{
    public class ChatlogConfiguration : IEntityTypeConfiguration<Chatlog>
    {
        public void Configure(EntityTypeBuilder<Chatlog> builder)
        {
            builder.ToTable("Chatlogs");
        }
    }
}
