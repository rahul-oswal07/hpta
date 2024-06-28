using System.ComponentModel.DataAnnotations.Schema;

namespace HPTA.Data.Entities
{
    public class AIResponse
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Team))]
        public int? TeamId { get; set; }

        public Team Team { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Survey))]
        public int? SurveyId { get; set; }

        public Survey Survey { get; set; }

        public AIResponseData ResponseData { get; set; }
    }

    public class AIResponseData
    {
        public string Description { get; set; }
        public List<AIRecommendation> Recommendations { get; set; }
    }

    public class AIRecommendation
    {
        public string Category { get; set; }
        public string Score { get; set; }
        public string Description { get; set; }
    }
}
