using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<List<UspTeamDataReturnModel>> LoadChartData(int teamId);

    Task<List<TeamModel>> GetAllTeams();
}
