namespace HPTA.DTO;

public class UspTeamDataReturnModel
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double Average { get; set; }

    public int RespondedUsers { get;set; }

    public int TotalUsers { get; set; }
}

public class TeamDataModel
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public List<ScoreModel> Scores { get; set; }

    public string PromptData { get; set; }

    public int RespondedUsers { get; set; }

    public int TotalUsers { get; set; }
}

public class ScoreModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double Average { get; set; }
}
