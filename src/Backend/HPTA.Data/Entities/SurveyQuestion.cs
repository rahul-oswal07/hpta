using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities
{
    [PrimaryKey(nameof(SurveyId), nameof(QuestionNumber))]
    public partial class SurveyQuestion
    {
        public int QuestionNumber { get; set; }

        #region Audit Properties
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        #endregion

        #region Navigation Properties
        [ForeignKey(nameof(Question))]
        public int QuestionId { get; set; }

        public Question Question { get; set; }

        [ForeignKey(nameof(Survey))]
        public int SurveyId { get; set; }

        public Survey Survey { get; set; }

        public ICollection<Answer> Answers { get; set; }
        #endregion
    }
}
