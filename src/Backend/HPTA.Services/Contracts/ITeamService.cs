using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int teamId);

    Task<List<TeamModel>> GetAllTeams();
}
