using IdentityServer4.EntityFramework.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatJS.WebServer.Configurations.Identity
{
    public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
    {
        public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
        {
            builder.ToTable("DeviceFlowCodes");
        }
    }
}
