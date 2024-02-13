using HPTA.Data.Configurations.Helpers;
using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations;

public class SurveyQuestionConfiguration : IEntityTypeConfiguration<SurveyQuestion>
{
    public void Configure(EntityTypeBuilder<SurveyQuestion> builder)
    {
        builder.HasIndex(p => new { p.SurveyId, p.QuestionId }).IsUnique().HasFilter("[IsDeleted] = 0");

#if DEBUG
        var data = SeedHelper.SeedData<Question>("questions.json").Select((q, i) => new SurveyQuestion { SurveyId = 1, QuestionId = q.Id, QuestionNumber = i + 1 }).ToList();
        builder.HasData(data);
#endif
    }
}
