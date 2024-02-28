using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int? teamId);

    Task<List<TeamModel>> GetAllTeams();

    Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId);

    Task<List<string>> ListTeamMembers(int teamId);
}
