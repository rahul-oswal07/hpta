using HPTA.Common;

namespace HPTA.DTO
{
    public class SurveyAnswerModel
    {
        public int QuestionNumber { get; set; }

        public RatingValue Rating { get; set; }
    }
}
