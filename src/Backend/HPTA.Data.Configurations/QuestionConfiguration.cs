using HPTA.Data.Configurations.Helpers;
using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
#if DEBUG

        var data = SeedHelper.SeedData<Question>("questions.json");
        builder.HasData(data);
#endif
    }
}
