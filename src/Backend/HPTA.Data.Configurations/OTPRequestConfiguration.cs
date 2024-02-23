using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    internal class OTPRequestConfiguration : IEntityTypeConfiguration<OTPRequest>
    {
        public void Configure(EntityTypeBuilder<OTPRequest> builder)
        {
            builder.HasIndex(p => p.Email).IsUnique();
        }
    }
}
