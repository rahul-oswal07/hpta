using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int teamId, string userId);

    Task<List<TeamModel>> GetAllTeams();

    Task<TeamDataModel> LoadCategoryChartData(int teamId, int categoryId, string userId);
}
