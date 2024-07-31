namespace HPTA.DTO;

public class UspTeamDataReturnModel
{
    public int? TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public int SurveyId { get; set; }

    public string SurveyName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double Average { get; set; }

    public int? RespondedUsers { get; set; }

    public int? TotalUsers { get; set; }
}

public class SurveyResultDataModel
{
    public int SurveyId { get; set; }

    public string SurveyName { get; set; }

    public List<ScoreModel> Scores { get; set; }

    public TeamPerformanceDTO TeamPerformance { get; set; }

    public int? RespondedUsers { get; set; }
}

public class PromptScoreDescriptionModel
{
    public string Description { get; set; } = string.Empty;

    public List<ScoreModel> Categories { get; set; }
}

public class TeamDataModel
{
    public int? TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public int? TotalUsers { get; set; }

    public List<SurveyResultDataModel> SurveyResults { get;set; }
}

public class ScoreModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double Average { get; set; }

    public string Description { get; set; } = string.Empty;
}

public class TeamPerformanceDTO
{
    public DateTime? AssessmentDateTime { get; set; }
    public string Description { get; set; }
    public List<CategoryDTO> Categories { get; set; }
}

public class CategoryDTO
{
    public string Category { get; set; }
    public decimal Score { get; set; }
    public string Description { get; set; }
}
