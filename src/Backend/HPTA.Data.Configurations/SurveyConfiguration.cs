using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    public class SurveyConfiguration : IEntityTypeConfiguration<Survey>
    {
        public void Configure(EntityTypeBuilder<Survey> builder)
        {
            builder.HasData(new Survey { Id = 1, Title = "General HPTA", IsActive = true });
        }
    }
}
