namespace HPTA.DTO;

public class SurveyQuestionModel
{
    public int QuestionNumber { get; set; }
    public string Question { get; set; } = string.Empty;
    public string SubCategory { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
