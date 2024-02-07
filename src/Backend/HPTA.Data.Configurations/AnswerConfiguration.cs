using HPTA.Common;
using HPTA.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HPTA.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.HasDiscriminator<QuestionAnswerType>("AnswerType")
                .HasValue<RatingAnswer>(QuestionAnswerType.Rating)
                .HasValue<FreeTextAnswer>(QuestionAnswerType.FreeText);
            builder.HasOne(a => a.Question).WithMany(q => q.Answers).HasForeignKey(f => new { f.SurveyId, f.QuestionNumber });
            builder.HasIndex(p => new { p.SurveyId, p.QuestionNumber, p.UserId }).IsUnique().HasFilter("[IsDeleted] = 0");
        }
    }
}
