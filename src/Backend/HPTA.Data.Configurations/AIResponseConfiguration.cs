using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace HPTA.Data.Configurations
{
    public class AIResponseConfiguration : IEntityTypeConfiguration<AIResponse>
    {
        public void Configure(EntityTypeBuilder<AIResponse> builder)
        {
            builder.HasIndex(p => p.UserId).IsUnique().HasFilter($"{nameof(AIResponse.UserId)} is not null");
            builder.HasIndex(p => p.TeamId).IsUnique().HasFilter($"{nameof(AIResponse.TeamId)} is not null");
            builder.Property(e => e.ResponseData).HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<AIResponseData>(v) ?? new AIResponseData()
                );
        }
    }
}
