using HPTA.DTO;

namespace HPTA.Services.Contracts;

public interface ITeamService
{
    Task<TeamDataModel> LoadChartData(int? teamId, int? surveyId);

    Task<List<TeamModel>> GetAllTeams();

    Task<TeamDataModel> LoadCategoryChartData(int? teamId, int categoryId, int? surveyId);

    Task<List<string>> ListTeamMembers(int teamId);
}
