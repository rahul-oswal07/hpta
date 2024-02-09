namespace HPTA.DTO;

public class UspTeamDataReturnModel
{
    public int TeamId { get; set; }

    public string TeamName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public double Average { get; set; }
}
